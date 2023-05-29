using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ScheduleFaculty.Core.Database;
using ScheduleFaculty.Core.Entities;
using ScheduleFaculty.Core.Services.Abstractions;
using ScheduleFaculty.Core.Utils;

namespace ScheduleFaculty.Core.Services;

public class CheckAvailabilityService : ICheckAvailabilityService
{
    private readonly ApplicationDbContext _dbContext;

    public CheckAvailabilityService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ActionResponse> ForClassroom(Guid classroomId, int startTime, int endTime, DayOfWeek dayOfWeek,
        List<int> studyWeeks, int semester)
    {
        var response = new ActionResponse();

        var classroomHours = await _dbContext.HourStudyOfAYears
            .Include(h => h.CourseHourType)
            .ThenInclude(c => c.Course)
            .Where(h => (h.ClassroomId == classroomId)
                        && (h.DayOfWeek == dayOfWeek)
                        && h.CourseHourType.Course.Semester == semester
            )
            .OrderBy(h => h.StartTime).ToListAsync();

        var classroomHourWithContainsStudyWeeks = new List<HourStudyOfAYear>();

        foreach (var classroomHour in classroomHours)
        {
            if (classroomHour.StudyWeeks.Intersect(studyWeeks).Any())
            {
                classroomHourWithContainsStudyWeeks.Add(classroomHour);
            }
        }

        foreach (var hour in classroomHourWithContainsStudyWeeks)
        {
            if (((hour.StartTime <= startTime) && (hour.EndTime > startTime)) ||
                ((hour.StartTime < endTime) && (hour.EndTime >= endTime)))
            {
                var occupiedWeeks = string.Join(", ", hour.StudyWeeks.Intersect(studyWeeks));
                response.AddError("Classroom is occupied in weeks: " + occupiedWeeks);
                return response;
            }
        }

        return response;
    }

    public async Task<ActionResponse<List<Guid>>> AvailableSemigroups(Guid courseHourTypeId, int startTime,
        int endTime, DayOfWeek dayOfWeek, List<int> studyWeeks)
    {
        var response = new ActionResponse<List<Guid>>();

        var course = await _dbContext.CourseHourTypes
            .Include(c => c.Course)
            .Where(c => c.Id == courseHourTypeId)
            .Select(c => c.Course).SingleOrDefaultAsync();

        var hourType = await _dbContext.CourseHourTypes
            .Include(c => c.HourType)
            .Where(h => h.Id == courseHourTypeId)
            .Select(h => h.HourType).SingleOrDefaultAsync();


        var semigroupsThatHaveCourseHourType = await _dbContext.GroupsOfAStudyHour
            .Include(g => g.HourStudyOfAYear)
            .Where(g => g.HourStudyOfAYear.CourseHourTypeId == courseHourTypeId)
            .Select(g => g.SemiGroupId)
            .ToListAsync();

        var semigroupsWithoutCourseHourType = await _dbContext.StudyYearGroups
            .Where(s => (s.StudyProgramYearId == course.StudyProgramYearId)
                        && (!semigroupsThatHaveCourseHourType.Contains(s.Id)))
            .Select(s=>s.Id)
            .ToListAsync();

        if ((hourType.NeedAllSemiGroups) && semigroupsThatHaveCourseHourType.Any())
        {
            response.AddError("This course with all semi groups exist");
            return response;
        }

        if ((hourType.NeedAllSemiGroups))
        {
            var checkIfAHourTypeThatNeedAllSemigroupsAlreadyOccupiedThisHours = await CheckIfStudyYearIsAvailable(
                course.StudyProgramYearId, startTime, endTime, dayOfWeek,
                studyWeeks, course.Semester,true);

            if (checkIfAHourTypeThatNeedAllSemigroupsAlreadyOccupiedThisHours.HasErrors())
            {
                response.AddError(checkIfAHourTypeThatNeedAllSemigroupsAlreadyOccupiedThisHours.Errors[0]);
                return response;
            }
            
            response.Item = semigroupsWithoutCourseHourType;
            return response;
        }

        if (course.IsOptional&&hourType.NeedAllSemiGroups)
        {
            var checkIfAHourTypeThatNeedAllSemigroupsAlreadyOccupiedThisHours = await CheckIfStudyYearIsAvailable(
                course.StudyProgramYearId, startTime, endTime, dayOfWeek,
                studyWeeks, course.Semester,false);

            if (checkIfAHourTypeThatNeedAllSemigroupsAlreadyOccupiedThisHours.HasErrors())
            {
                response.AddError(checkIfAHourTypeThatNeedAllSemigroupsAlreadyOccupiedThisHours.Errors[0]);
                return response;
            }
            response.Item = semigroupsWithoutCourseHourType;
            return response;
        }


        var semigroupsAvailable = new List<Guid>();
        foreach (var semigroup in semigroupsWithoutCourseHourType)
        {
            var checkIfAHourTypeThatNeedAllSemigroupsAlreadyOccupiedThisHours = await CheckIfStudyYearIsAvailable(
                course.StudyProgramYearId, startTime, endTime, dayOfWeek,
                studyWeeks, course.Semester,false);

            if (checkIfAHourTypeThatNeedAllSemigroupsAlreadyOccupiedThisHours.HasErrors())
            {
                response.AddError(checkIfAHourTypeThatNeedAllSemigroupsAlreadyOccupiedThisHours.Errors[0]);
                return response;
            }
            
            var semigroupToCheck = await CheckIfSemigroupIsAvailable(semigroup, startTime, endTime, dayOfWeek,
                studyWeeks, course.Semester);
            if ((!semigroupToCheck.HasErrors())&&(semigroupsAvailable.Count<hourType.SemiGroupsPerHour))
            {
                semigroupsAvailable.Add(semigroup);
            }
        }

        if (semigroupsAvailable.Count < hourType.SemiGroupsPerHour)
        {
            response.AddError("Don't find enough semigroups");
            return response;
        }

        response.Item = semigroupsAvailable;
        return response;
    }

    private async Task<ActionResponse> CheckIfSemigroupIsAvailable(Guid semigroupId, int startTime,
        int endTime, DayOfWeek dayOfWeek,
        List<int> studyWeeks, int semester)
    {
        var response = new ActionResponse();

        var semigroupHoursInTheSameInterval = await _dbContext.GroupsOfAStudyHour
            .Include(g => g.HourStudyOfAYear)
            .ThenInclude(h => h.CourseHourType.Course)
            .Where(g => (g.SemiGroupId == semigroupId)
                        && (g.HourStudyOfAYear.DayOfWeek == dayOfWeek)
                        && g.HourStudyOfAYear.CourseHourType.Course.Semester == semester
                        && g.HourStudyOfAYear.CourseHourType.Course.IsOptional==false
                        && (((g.HourStudyOfAYear.StartTime <= startTime) && (g.HourStudyOfAYear.EndTime > startTime)) ||
                            ((g.HourStudyOfAYear.StartTime < endTime) && (g.HourStudyOfAYear.EndTime >= endTime)))
            )
            .OrderBy(g => g.HourStudyOfAYear.StartTime).ToListAsync();

        foreach (var semigroupHour in semigroupHoursInTheSameInterval)
        {
            if (semigroupHour.HourStudyOfAYear.StudyWeeks.Intersect(studyWeeks).Any())
            {
                response.AddError("");
                return response;
            }
        }

        return response;
    }

    private async Task<ActionResponse> CheckIfStudyYearIsAvailable(Guid studyProgramYearId, int startTime,
        int endTime, DayOfWeek dayOfWeek,
        List<int> studyWeeks, int semester, bool checkNeedAllSemigroupsOptionals)
    {
        var response = new ActionResponse();
        
        var needAllSemigroupHours =  await _dbContext.HourStudyOfAYears
            .Include(h => h.CourseHourType.Course)
            .Include(h=>h.CourseHourType.HourType)
            .Where(h => 
                (h.CourseHourType.Course.StudyProgramYearId == studyProgramYearId)
                       && 
                (h.DayOfWeek == dayOfWeek)
                       && h.CourseHourType.Course.Semester == semester
                       && h.CourseHourType.HourType.NeedAllSemiGroups == true
                        && ((!checkNeedAllSemigroupsOptionals)?(h.CourseHourType.Course.IsOptional==false):true)//vezi
                         &&(h.StartTime <= startTime)
                         &&(h.EndTime > startTime)
                         &&(h.StartTime < endTime)
                         &&(h.EndTime >= endTime)
                       && (((h.StartTime <= startTime) && (h.EndTime > startTime)) ||
                           ((h.StartTime < endTime) && (h.EndTime >= endTime)))
            )
            .OrderBy(h => h.StartTime).ToListAsync();
        
        foreach (var studyWeeksOfCourse in needAllSemigroupHours)
        {
            if (studyWeeksOfCourse.StudyWeeks.Intersect(studyWeeks).Any())
            {
                var occupiedWeeks = string.Join(", ", studyWeeksOfCourse.StudyWeeks.Intersect(studyWeeks));
                response.AddError("Study year are is occupied in weeks: " + occupiedWeeks);
                return response;
            }
        }

        return response;
    }
}