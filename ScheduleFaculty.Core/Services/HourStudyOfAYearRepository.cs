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
    private readonly ICheckAvailabilityService _checkAvailability;

    public HourStudyOfAYearRepository(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager,
        ICheckAvailabilityService checkAvailability)
    {
        _dbContext = dbContext;
        _userManager = userManager;
        _checkAvailability = checkAvailability;
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

    public async Task<ActionResponse<List<HourStudyOfAYear>>> GetByStudyYearSemiGroupId(Guid semiGroupId)
    {
        var response = new ActionResponse<List<HourStudyOfAYear>>();
        var semiGroupsOfStudyHour =
            await _dbContext.GroupsOfAStudyHour.Include(g => g.HourStudyOfAYear)
                .Where(g => g.SemiGroupId == semiGroupId).ToListAsync();

        if (semiGroupsOfStudyHour.Count == 0)
        {
            response.AddError("This semigroup don't have a hour study");
            return response;
        }

        var hourStudy = new List<HourStudyOfAYear>();

        foreach (var hour in semiGroupsOfStudyHour)
        {
            hourStudy.Add(hour.HourStudyOfAYear);
        }

        response.Item = hourStudy;
        return response;
    }

    public async Task<ActionResponse<List<HourStudyOfAYear>>> GetByCourseId(Guid courseId)
    {
        var response = new ActionResponse<List<HourStudyOfAYear>>();
        var hoursStudyOfAYear =
            await _dbContext.HourStudyOfAYears.Include(h => h.CourseHourType.Course)
                .Where(h => h.CourseHourType.Course.Id == courseId).ToListAsync();

        if (hoursStudyOfAYear.Count == 0)
        {
            response.AddError("This course don't have a hour study");
            return response;
        }

        response.Item = hoursStudyOfAYear;
        return response;
    }

    public async Task<ActionResponse<List<HourStudyOfAYear>>> GetByHourTypeId(Guid hourTypeId)
    {
        var response = new ActionResponse<List<HourStudyOfAYear>>();
        var hoursStudyOfAYear =
            await _dbContext.HourStudyOfAYears.Include(h => h.CourseHourType.HourType)
                .Where(h => h.CourseHourType.HourType.Id == hourTypeId).ToListAsync();

        if (hoursStudyOfAYear.Count == 0)
        {
            response.AddError("This course don't have a hour study");
            return response;
        }

        response.Item = hoursStudyOfAYear;
        return response;
    }

    public async Task<ActionResponse<List<HourStudyOfAYear>>> GetByUserId(string userId)
    {
        var response = new ActionResponse<List<HourStudyOfAYear>>();
        var hoursStudyOfAYear =
            await _dbContext.HourStudyOfAYears.Where(h => h.UserId == userId).ToListAsync();

        if (hoursStudyOfAYear.Count == 0)
        {
            response.AddError("This user don't have a hour study");
            return response;
        }

        response.Item = hoursStudyOfAYear;
        return response;
    }

    public async Task<ActionResponse<List<HourStudyOfAYear>>> GetByClassroomId(Guid classroomId)
    {
        var response = new ActionResponse<List<HourStudyOfAYear>>();
        var hoursStudyOfAYear =
            await _dbContext.HourStudyOfAYears.Where(h => h.ClassroomId == classroomId).ToListAsync();

        if (hoursStudyOfAYear.Count == 0)
        {
            response.AddError("This classroom don't have a hour study");
            return response;
        }

        response.Item = hoursStudyOfAYear;
        return response;
    }

    public async Task<ActionResponse<List<HourStudyOfAYear>>> GetAllHourStudyOfAYear()
    {
        var response = new ActionResponse<List<HourStudyOfAYear>>();
        var hoursStudyOfAYear = await _dbContext.HourStudyOfAYears.ToListAsync();

        response.Item = hoursStudyOfAYear;
        return response;
    }

    public async Task<ActionResponse<HourStudyOfAYear>> CreateHourStudyOfAYear(Guid courseHourTypeId,
        string userId,
        Guid classroomId, List<int> studyWeeks, int startTime,
        int endTime, DayOfWeek dayOfWeek)
    {
        var response = new ActionResponse<HourStudyOfAYear>();
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
        if (activeStatus is null || activeStatus.Name is "Course" or "NoEditable")
        {
            response.AddError("It's not period!");
            return response;
        }

        if (course.Semester != activeStatus.Semester)
        {
            response.AddError("It's not semester");
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

        var checkTermOfClassroom =
            await _checkAvailability.ForClassroom(classroomId, startTime, endTime, dayOfWeek, studyWeeks,
                course.Semester);
        if (checkTermOfClassroom.HasErrors())
        {
            response.AddError(checkTermOfClassroom.Errors[0]);
            return response;
        }

        var semigroupsAvailable =
            await _checkAvailability.AvailableSemigroups(courseHourTypeId, startTime, endTime, dayOfWeek, studyWeeks);

        if (semigroupsAvailable.HasErrors())
        {
            response.AddError(semigroupsAvailable.Errors[0]);
            return response;
        }

        var dbAddHourStudyOfAYear = new HourStudyOfAYear
        {
            Id = Guid.NewGuid(),
            CourseHourTypeId = courseHourTypeId,
            UserId = userId,
            ClassroomId = classroomId,
            StudyWeeks = studyWeeks,
            DayOfWeek = dayOfWeek,
            StartTime = startTime,
            EndTime = endTime,
        };

        await _dbContext.HourStudyOfAYears.AddAsync(dbAddHourStudyOfAYear);

        if (semigroupsAvailable.Item.Any())
        {
            foreach (var semigroup in semigroupsAvailable.Item)
            {
                var dbSemigroupToAdd = new GroupsOfAStudyHour
                {
                    HourStudyOfAYearId = dbAddHourStudyOfAYear.Id,
                    SemiGroupId = semigroup
                };
                await _dbContext.GroupsOfAStudyHour.AddAsync(dbSemigroupToAdd);
            }
        }

        await _dbContext.SaveChangesAsync();
        response.Item = dbAddHourStudyOfAYear;


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