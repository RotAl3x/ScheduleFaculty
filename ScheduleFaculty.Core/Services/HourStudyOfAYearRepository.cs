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
            await _dbContext.HourStudyOfAYears.Where(h => h.SemiGroupId == semiGroupId).ToListAsync();

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

    private async Task<ActionResponse<bool>> CheckClassroomIsFree(Guid classroomId, DateTime startTime,
        DateTime endTime,
        List<int> studyWeeks)
    {
        var response = new ActionResponse<bool>();
        var classroomHour = await _dbContext.HourStudyOfAYears
            .Where(h => (h.ClassroomId == classroomId) && (h.DayOfWeek == startTime.DayOfWeek))
            .OrderBy(h => h.StartTime).ToListAsync();

        var classroomHourThatContainsStudyWeeks = new List<HourStudyOfAYear>();

        foreach (var classroomOccupiedWeek in classroomHour)
        {
            var freeHourForStudyWeeks = classroomOccupiedWeek.StudyWeeks.Intersect(studyWeeks);
            if (freeHourForStudyWeeks.Any())
            {
                classroomHourThatContainsStudyWeeks.Add(classroomOccupiedWeek);
            }
        }

        if (classroomHourThatContainsStudyWeeks[0].StartTime >= endTime ||
            (classroomHourThatContainsStudyWeeks.Count == 0))
        {
            response.Item = true;
            return response;
        }

        for (var i = 0; i < classroomHourThatContainsStudyWeeks.Count; i++)
        {
            if (classroomHourThatContainsStudyWeeks[i].EndTime <= startTime)
            {
                if (classroomHourThatContainsStudyWeeks[i + 1].StartTime >= endTime ||
                    (i == classroomHourThatContainsStudyWeeks.Count - 1))
                {
                    response.Item = true;
                    return response;
                }
            }
        }

        foreach (var hour in classroomHourThatContainsStudyWeeks)
        {
            if ((hour.StartTime >= startTime) && (hour.EndTime <= startTime))
            {
                var occupiedWeeks = string.Join(", ", hour.StudyWeeks.Intersect(studyWeeks));
                response.AddError("Classroom is occupied in weeks: " + occupiedWeeks);
                return response;
            }
        }

        response.Item = true;
        return response;
    }
    
    public async Task<ActionResponse<HourStudyOfAYear>> CreateHourStudyOfAYear(Guid courseHourTypeId, string userId,
        Guid classroomId, List<int> studyWeeks, DateTime startTime,
        DateTime endTime, bool searchForSemiGroupWithFreeHourAfterStudy)
    {
        var response = new ActionResponse<HourStudyOfAYear>();
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

        var courseHourType = await _dbContext.CourseHourTypes.SingleOrDefaultAsync(c => c.Id == courseHourTypeId);

        if (courseHourType is null)
        {
            response.AddError("Course hour doesn't exist");
            return response;
        }

        var hourType = await _dbContext.HourTypes.SingleOrDefaultAsync(h => h.Id == courseHourType.HourTypeId);
        var course = await _dbContext.Courses.SingleOrDefaultAsync(c => c.Id == courseHourType.CourseId);
        var studyProgram = await _dbContext.StudyPrograms.SingleOrDefaultAsync(s => s.Id == course.StudyProgramYearId);
        var studyYearGroups = await _dbContext.StudyYearGroups.Where(s => s.StudyProgramYearId == studyProgram.Id)
            .ToListAsync();
        
        var checkTermOfClassroom = await CheckClassroomIsFree(classroomId, startTime, endTime, studyWeeks);
        if (checkTermOfClassroom.HasErrors())
        {
            response.AddError(string.Join("",checkTermOfClassroom.Errors));
            return response;
        }
        
        var semigroupOccupied = 0;

        return response;
    }


    public Task<ActionResponse<HourStudyOfAYear>> EditHourStudyOfAYear(Guid id, Guid semiGroup, Guid courseHourTypeId,
        string userId, Guid classroomId,
        List<int> studyWeeks, DateTime startTime, DateTime endTime)
    {
        throw new NotImplementedException();
    }

    public Task<ActionResponse<HourStudyOfAYear>> DeleteHourStudyOfAYear(Guid id)
    {
        throw new NotImplementedException();
    }
}