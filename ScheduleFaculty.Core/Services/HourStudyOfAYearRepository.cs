using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ScheduleFaculty.Core.Database;
using ScheduleFaculty.Core.Entities;
using ScheduleFaculty.Core.Services.Abstractions;
using ScheduleFaculty.Core.Utils;

namespace ScheduleFaculty.Core.Services;

public class HourStudyOfAYearRepository : IHourStudyOfAYearRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly UserManager<ApplicationUser> _userManager;

    public HourStudyOfAYearRepository(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager)
    {
        _dbContext = dbContext;
        _userManager = userManager;
    }

    public async Task<ActionResponse<HourStudyOfAYear>> GetById(Guid id)
    {
        var response = new ActionResponse<HourStudyOfAYear>();
        var hourStudyOfAYear = await _dbContext.HourStudyOfAYears.SingleOrDefaultAsync(h => h.Id == id);

        if (hourStudyOfAYear is null)
        {
            response.AddError("Hour study doesn't exist");
            return response;
        }

        response.Item = hourStudyOfAYear;
        return response;
    }

    public async Task<ActionResponse<List<HourStudyOfAYear>>> GetByStudyYearGroupId(Guid semiGroupId)
    {
        var response = new ActionResponse<List<HourStudyOfAYear>>();
        var hoursStudyOfAYear =
            await _dbContext.HourStudyOfAYears.Where(h => h.SemiGroupId.Contains(semiGroupId)).ToListAsync();

        if (hoursStudyOfAYear.Count == 0)
        {
            response.AddError("This semigroup don't have a hour study");
            return response;
        }

        response.Item = hoursStudyOfAYear;
        return response;
    }

    public async Task<ActionResponse<List<HourStudyOfAYear>>> GetByCourseId(Guid courseId)
    {
        throw new NotImplementedException();
        // var response = new ActionResponse<List<HourStudyOfAYear>>();
        // var courseHourType = await _dbContext.CourseHourTypes.SingleOrDefaultAsync()
        //     var hoursStudyOfAYear =
        //     // await _dbContext.HourStudyOfAYears.Where(h => h.SemiGroupId == semiGroupId).ToListAsync();
        //
        // if (hoursStudyOfAYear.Count == 0)
        // {
        //     response.AddError("This semigroup don't have a course");
        //     return response;
        // }
        //
        // response.Item = hoursStudyOfAYear;
        // return response;
    }

    public Task<ActionResponse<List<HourStudyOfAYear>>> GetByHourTypeId(Guid hourTypeId)
    {
        throw new NotImplementedException();
    }

    public Task<ActionResponse<List<HourStudyOfAYear>>> GetByUserId(string userId)
    {
        throw new NotImplementedException();
    }

    public Task<ActionResponse<List<HourStudyOfAYear>>> GetByClassroomId(Guid classroomId)
    {
        throw new NotImplementedException();
    }

    public Task<ActionResponse<List<HourStudyOfAYear>>> GetAllHourStudyOfAYear()
    {
        throw new NotImplementedException();
    }

    private async Task<ActionResponse> CheckClassroomIsFree(Guid classroomId, int startTime,
        int endTime, DayOfWeek dayOfWeek,
        List<int> studyWeeks, int semester)
    {
        var response = new ActionResponse();
        var classroomHour = await _dbContext.HourStudyOfAYears
            .Include(h => h.CourseHourType.Course)
            .Where(h => (h.ClassroomId == classroomId)
                        && (h.DayOfWeek == dayOfWeek)
                        && h.CourseHourType.Course.Semester == semester
                        && h.StudyWeeks.Intersect(studyWeeks).Any())
            .OrderBy(h => h.StartTime).ToListAsync();


        foreach (var hour in classroomHour)
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

    private async Task<ActionResponse> CheckIfSemigroupIsFree(Guid semigroupId, int startTime,
        int endTime, DayOfWeek dayOfWeek,
        List<int> studyWeeks, int semester)
    {
        var response = new ActionResponse();

        var semigroupHour = await _dbContext.HourStudyOfAYears
            .Include(h => h.CourseHourType.Course)
            .Where(h => (h.SemiGroupId.Contains(semigroupId))
                        && (h.DayOfWeek == dayOfWeek)
                        && h.CourseHourType.Course.Semester == semester
                        && !h.CourseHourType.Course.IsOptional
                        && h.StudyWeeks.Intersect(studyWeeks).Any())
            .OrderBy(h => h.StartTime).ToListAsync();


        foreach (var hour in semigroupHour)
        {
            if (((hour.StartTime <= startTime) && (hour.EndTime > startTime)) ||
                ((hour.StartTime < endTime) && (hour.EndTime >= endTime)))
            {
                response.AddError("");
                return response;
            }
        }

        return response;
    }

    public async Task<ActionResponse<List<HourStudyOfAYear>>> CreateHourStudyOfAYear(Guid courseHourTypeId,
        string userId,
        Guid classroomId, List<int> studyWeeks, int startTime,
        int endTime, DayOfWeek dayOfWeek)
    {
        var response = new ActionResponse<List<HourStudyOfAYear>>();
        if (startTime >= 24 || endTime >= 24)
        {
            response.AddError("A day have max 24 hours");
            return response;
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
        {
            response.AddError("User does not exist!");
            return response;
        }

        var classroom = await _dbContext.Classrooms.SingleOrDefaultAsync(c => c.Id == classroomId);
        if (classroom is null)
        {
            response.AddError("Classroom doesn't exist");
            return response;
        }

        if (!classroom.DaysOfWeek.Contains(dayOfWeek))
        {
            response.AddError("Classroom is not free in that day");
            return response;
        }

        var courseHourType = await _dbContext.CourseHourTypes
            .Include(c => c.HourType)
            .Include(c => c.Course)
            .ThenInclude(c => c.StudyProgram)
            .SingleOrDefaultAsync(c => c.Id == courseHourTypeId);

        if (courseHourType is null)
        {
            response.AddError("Course hour doesn't exist");
            return response;
        }

        if (studyWeeks.Count * (endTime - startTime) > courseHourType.TotalHours)
        {
            response.AddError("Total hours of course is full");
            return response;
        }

        var course = courseHourType.Course;
        var studyProgram = course.StudyProgram;
        var hourType = courseHourType.HourType;
        
        if (studyWeeks.Max() > studyProgram.WeeksInASemester)
        {
            response.AddError("Study weeks are more than weeks in a semester");
            return response;
        }

        var activeStatus = await _dbContext.Statuses.SingleOrDefaultAsync(s => s.IsActive == true);
        if (activeStatus is null||activeStatus.Name is "Course" or "NoEditable")
        {
            response.AddError("It's not period!");
            return response;
        }

        if (course.Semester != activeStatus.Semester)
        {
            response.AddError("It's not semester");
            return response;
        }

        var checkTermOfClassroom =
            await CheckClassroomIsFree(classroomId, startTime, endTime, dayOfWeek, studyWeeks, course.Semester);
        if (checkTermOfClassroom.HasErrors())
        {
            response.AddError(checkTermOfClassroom.Errors[0]);
            return response;
        }

        var dbAdd = new HourStudyOfAYear
        {
            CourseHourTypeId = courseHourTypeId,
            UserId = userId,
            ClassroomId = classroomId,
            StudyWeeks = studyWeeks,
            DayOfWeek = dayOfWeek,
            StartTime = startTime,
            EndTime = endTime,
        };
        
        
        if (!course.IsOptional)
        {
            var hourStudyGuidSemigroupThatHaveCourseType = await _dbContext.HourStudyOfAYears
                .Where(h => h.CourseHourTypeId == courseHourTypeId)
                .SelectMany(h => h.SemiGroupId)
                .ToListAsync();

            var studyYearGroupsWithoutSemigroupsThatHaveCourseType = await _dbContext.StudyYearGroups
                .Where(s => (s.StudyProgramYearId == studyProgram.Id)
                            && (!hourStudyGuidSemigroupThatHaveCourseType.Contains(s.Id)))
                .ToListAsync();
            var semigroupOccupied = 0;
            var dbAddSemigroups = new List<Guid>();
            foreach (var semigroup in studyYearGroupsWithoutSemigroupsThatHaveCourseType)
            {
                if (semigroupOccupied < hourType.SemiGroupsPerHour)
                {
                    var semigroupAdd =
                        await CheckIfSemigroupIsFree(semigroup.Id, startTime, endTime, dayOfWeek, studyWeeks,
                            course.Semester);
                    if (!(semigroupAdd.HasErrors()))
                    {
                        semigroupOccupied++;
                        dbAddSemigroups.Add(semigroup.Id);
                    }
                    else
                    {
                        if (hourType.NeedAllSemiGroups)
                        {
                            response.AddError("Insufficient semigroups");
                            return response;
                        }
                    }
                }
            }

            if (semigroupOccupied < hourType.SemiGroupsPerHour)
            {
                response.AddError("Insufficient semigroups");
                return response;
            }

            dbAdd.SemiGroupId = dbAddSemigroups;
            await _dbContext.HourStudyOfAYears.AddAsync(dbAdd);

            await _dbContext.SaveChangesAsync();
            return response;
        }

        var hoursCoursesNotOptional = await _dbContext.HourStudyOfAYears
            .Include(h=>h.CourseHourType.HourType)
            .Include(h=>h.CourseHourType.Course)
            .Where(h => (!h.CourseHourType.Course.IsOptional)
                        && h.CourseHourType.HourType.NeedAllSemiGroups
                        && h.DayOfWeek==dayOfWeek
                        && h.CourseHourType.Course.Semester == course.Semester
                        && h.StudyWeeks.Intersect(studyWeeks).Any()
                        && h.CourseHourType.Course.StudyProgramYearId == studyProgram.Id)
            .ToListAsync();
        foreach (var hour in hoursCoursesNotOptional)
        {
            if (((hour.StartTime <= startTime) && (hour.EndTime > startTime)) ||
                ((hour.StartTime < endTime) && (hour.EndTime >= endTime)))
            {
                var occupiedWeeks = string.Join(", ", hour.StudyWeeks.Intersect(studyWeeks));
                response.AddError("This year is occupied in weeks: " + occupiedWeeks);
                return response;
            }
        }
        
        await _dbContext.HourStudyOfAYears.AddAsync(dbAdd);

        await _dbContext.SaveChangesAsync();
        return response;
    }


    public Task<ActionResponse<HourStudyOfAYear>> EditHourStudyOfAYear(Guid id, Guid semiGroup, Guid courseHourTypeId,
        string userId, Guid classroomId,
        List<int> studyWeeks, DateTime startTime, DateTime endTime)
    {
        throw new NotImplementedException();
    }

    public async Task<ActionResponse> DeleteHourStudyOfAYear(Guid id)
    {
        var response = new ActionResponse();
        var hourToDelete = await _dbContext.HourStudyOfAYears.SingleOrDefaultAsync(h => h.Id == id);
        if (hourToDelete is null)
        {
            response.AddError("Hour study of a year doesn't exist");
            return response;
        }

        return response;
    }
}