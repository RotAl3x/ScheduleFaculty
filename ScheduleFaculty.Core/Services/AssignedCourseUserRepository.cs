using Microsoft.EntityFrameworkCore;
using ScheduleFaculty.Core.Database;
using ScheduleFaculty.Core.Entities;
using ScheduleFaculty.Core.Services.Abstractions;
using ScheduleFaculty.Core.Utils;

namespace ScheduleFaculty.Core.Services;

public class AssignedCourseUserRepository:IAssignedCourseUserRepository
{
    
    private readonly ApplicationDbContext _dbContext;

    public AssignedCourseUserRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    
    public async Task<ActionResponse<List<AssignedCourseUser>>> GetAllAssignedCourseUser()
    {
        var response = new ActionResponse<List<AssignedCourseUser>>();
        var assignedCoursesUsers = await _dbContext.AssignedCourseUsers.Include(a=>a.Course).ToListAsync();

        response.Item = assignedCoursesUsers;
        return response;
    }

    public async Task<ActionResponse<AssignedCourseUser>> GetById(Guid id)
    {
        var response = new ActionResponse<AssignedCourseUser>();
        var assignedCoursesUsers =
            await _dbContext.AssignedCourseUsers.SingleOrDefaultAsync(a => a.Id == id);
        
        if (assignedCoursesUsers is null)
        {
            response.AddError("Assigned doesn't exists");
            return response;
        }

        response.Item = assignedCoursesUsers;
        return response;
    }

    public async Task<ActionResponse<List<AssignedCourseUser>>> GetAssignedCourseUserByCourseId(Guid courseId)
    {
        var response = new ActionResponse<List<AssignedCourseUser>>();
        var assignedCoursesUsers =
            await _dbContext.AssignedCourseUsers.Where(a => a.CourseId == courseId).Include(a=>a.Course).ToListAsync();

        response.Item = assignedCoursesUsers;
        return response;
    }

    public async Task<ActionResponse<List<AssignedCourseUser>>> GetAssignedCourseUsersByProfessorId(string professorId)
    {
        var response = new ActionResponse<List<AssignedCourseUser>>();
        var assignedCoursesUsers =
            await _dbContext.AssignedCourseUsers.Where(a => a.ProfessorUserId == professorId).Include(a=>a.Course).ToListAsync();

        response.Item = assignedCoursesUsers;
        return response;
    }

    public async Task<ActionResponse<AssignedCourseUser>> CreateAssignedCourseUser(Guid courseId, string professorId)
    {
        var response = new ActionResponse<AssignedCourseUser>();
        var exists = await _dbContext.AssignedCourseUsers
            .Where(a => a.CourseId == courseId && a.ProfessorUserId == professorId).ToListAsync();
        if (exists.Count > 0)
        {
            response.AddError("Assigned already exists");
            return response;
        }

        var assignedCourseUser = new AssignedCourseUser
        {
            CourseId = courseId,
            ProfessorUserId = professorId
        };
        var dbAssignedCourseUser = await _dbContext.AssignedCourseUsers.AddAsync(assignedCourseUser);
        await _dbContext.SaveChangesAsync();

        response.Item = dbAssignedCourseUser.Entity;
        return response;
        
    }

    public async Task<ActionResponse<AssignedCourseUser>> EditAssignedCourseUser(Guid id,Guid courseId, string professorId)
    {
        var response = new ActionResponse<AssignedCourseUser>();
        var assignedCourseUser =
            await _dbContext.AssignedCourseUsers.SingleOrDefaultAsync(a =>
                a.ProfessorUserId == professorId && a.CourseId == courseId);
        
        if (assignedCourseUser is null)
        {
            response.AddError("Assigned Course User doesn't exist");
            return response;
        }

        assignedCourseUser.CourseId = courseId;
        assignedCourseUser.ProfessorUserId = professorId;
        await _dbContext.SaveChangesAsync();
        response.Item = assignedCourseUser;
        return response;
    }

    public async Task<ActionResponse> DeleteAssignedCourseUser(Guid id)
    {
        var response = new ActionResponse();

        var assignedCourseUsersToDelete = await _dbContext.AssignedCourseUsers.SingleOrDefaultAsync(a => a.Id == id);
        
        if (assignedCourseUsersToDelete is null)
        {
            response.AddError("Assigned Course Users doesn't exist");
            return response;
        }

        _dbContext.AssignedCourseUsers.Remove(assignedCourseUsersToDelete);
        await _dbContext.SaveChangesAsync();

        return response;
    }
}