using Microsoft.EntityFrameworkCore;
using ScheduleFaculty.Core.Database;
using ScheduleFaculty.Core.Entities;
using ScheduleFaculty.Core.Services.Abstractions;
using ScheduleFaculty.Core.Utils;

namespace ScheduleFaculty.Core.Services;

public class StudyProgramRepository : IStudyProgramRepository
{
    
    private readonly ApplicationDbContext _dbContext;

    public StudyProgramRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<ActionResponse<StudyProgram>> GetStudyProgram(Guid id)
    {
        var response = new ActionResponse<StudyProgram>();
        var studyProgram = await _dbContext.StudyPrograms.SingleOrDefaultAsync(c => c.Id == id);

        if (studyProgram == null)
        {
            response.AddError("Study program doesn't exist");
            return response;
        }

        response.Item = studyProgram;
        return response;
    }

    public async Task<ActionResponse<List<StudyProgram>>> GetAllStudyProgramS()
    {
        var response = new ActionResponse<List<StudyProgram>>();
        var studyPrograms = await _dbContext.StudyPrograms.ToListAsync();

        response.Item = studyPrograms;
        return response;
    }

    public async Task<ActionResponse<StudyProgram>> CreateStudyProgram(string name, int year)
    {
        var response = new ActionResponse<StudyProgram>();

        var nameExists = await _dbContext.StudyPrograms.AnyAsync(c => c.Name == name);

        if (nameExists)
        {
            response.AddError("Study program with the same name already exists");
            return response;
        }

        var studyProgram= new StudyProgram { Name = name, Year = year };
        var dbStudyProgram = await _dbContext.StudyPrograms.AddAsync(studyProgram);
        await _dbContext.SaveChangesAsync();

        response.Item = dbStudyProgram.Entity;
        return response;
    }

    public async Task<ActionResponse<StudyProgram>> EditStudyProgram(Guid id, string name, int year)
    {
        var response = new ActionResponse<StudyProgram>();

        var studyProgramToChange = await _dbContext.StudyPrograms.SingleOrDefaultAsync(c => c.Id == id);

        if (studyProgramToChange is null)
        {
            response.AddError("Study program doesn't exist");
            return response;
        }

        studyProgramToChange.Name = name;
        studyProgramToChange.Year = year;
        await _dbContext.SaveChangesAsync();
        response.Item = studyProgramToChange;
        return response;
    }

    public async Task<ActionResponse> DeleteStudyProgram(Guid id)
    {
        var response = new ActionResponse();

        var studyProgramToDelete = await _dbContext.StudyPrograms.SingleOrDefaultAsync(c => c.Id == id);
        
        if (studyProgramToDelete is null)
        {
            response.AddError("Study program doesn't exist");
            return response;
        }

        _dbContext.StudyPrograms.Remove(studyProgramToDelete);
        await _dbContext.SaveChangesAsync();

        return response;
    }

    public async Task SaveStudyProgram()
    {
        await _dbContext.SaveChangesAsync();
    }
}