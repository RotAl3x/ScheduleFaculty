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

    public async Task<ActionResponse<StudyProgram>> CreateStudyProgram(string name, int year, int weeksInASemester,
        int numberOfSemiGroups, int howManySemiGroupsAreInAGroup)
    {
        var response = new ActionResponse<StudyProgram>();

        var nameExists = await _dbContext.StudyPrograms.AnyAsync(c => c.Name == name);

        if (nameExists)
        {
            response.AddError("Study program with the same name already exists");
            return response;
        }

        var studyProgram = new StudyProgram { Name = name, Year = year, WeeksInASemester = weeksInASemester, NumberOfSemiGroups = numberOfSemiGroups, HowManySemiGroupsAreInAGroup = howManySemiGroupsAreInAGroup};
        var dbStudyProgram = await _dbContext.StudyPrograms.AddAsync(studyProgram);
        
        var semiGroup = new List<StudyYearGroup>();

        for (var i = 0; i < numberOfSemiGroups; i++)
        {
            semiGroup.Add(new StudyYearGroup
            {
                SemiGroup = i + 1,
                Group = i / howManySemiGroupsAreInAGroup + 1,
                Name = i / howManySemiGroupsAreInAGroup + 1 + "." + (i % howManySemiGroupsAreInAGroup + 1),
                StudyProgramYearId = studyProgram.Id
            });
        }

        await _dbContext.StudyYearGroups.AddRangeAsync(semiGroup);
        await _dbContext.SaveChangesAsync();

        response.Item = dbStudyProgram.Entity;
        return response;
    }

    public async Task<ActionResponse<StudyProgram>> EditStudyProgram(Guid id, string name, int year,
        int weeksInASemester,int numberOfSemiGroups,int howManySemiGroupsAreInAGroup)
    {
        var response = new ActionResponse<StudyProgram>();

        var studyProgramToChange = await _dbContext.StudyPrograms.SingleOrDefaultAsync(c => c.Id == id);
        
        if (studyProgramToChange is null)
        {
            response.AddError("Study program doesn't exist");
            return response;
        }
        
        var studyYearGroups = await _dbContext.StudyYearGroups
            .Where(s => s.StudyProgramYearId == id).OrderBy(s => s.SemiGroup).ToListAsync();
        
        var semiGroupToAdd = new List<StudyYearGroup>();
        
        if (studyProgramToChange.HowManySemiGroupsAreInAGroup != howManySemiGroupsAreInAGroup)
        {
            for (var i = 0; i < studyProgramToChange.NumberOfSemiGroups; i++)
            {
                studyYearGroups[i].Name =
                    i / howManySemiGroupsAreInAGroup + 1 + "." + (i % howManySemiGroupsAreInAGroup + 1);
                studyYearGroups[i].Group = i / howManySemiGroupsAreInAGroup + 1;
            }
        }
        
        if (studyProgramToChange.NumberOfSemiGroups < numberOfSemiGroups)
        {
            for (var i = studyProgramToChange.NumberOfSemiGroups; i < numberOfSemiGroups; i++)
            {
                semiGroupToAdd.Add(new StudyYearGroup
                {
                    SemiGroup = i+1 ,
                    Group = i / howManySemiGroupsAreInAGroup + 1,
                    Name = i / howManySemiGroupsAreInAGroup + 1 + "." + (i % howManySemiGroupsAreInAGroup + 1),
                    StudyProgramYearId = id
                });
            }
            await _dbContext.StudyYearGroups.AddRangeAsync(semiGroupToAdd);
            studyYearGroups.AddRange(semiGroupToAdd);
        }

        if (studyProgramToChange.NumberOfSemiGroups > numberOfSemiGroups)
        {
            _dbContext.StudyYearGroups.RemoveRange(studyYearGroups.GetRange(numberOfSemiGroups,studyProgramToChange.NumberOfSemiGroups- numberOfSemiGroups));
            studyYearGroups.RemoveRange(numberOfSemiGroups,studyProgramToChange.NumberOfSemiGroups- numberOfSemiGroups);
        }

        studyProgramToChange.NumberOfSemiGroups = numberOfSemiGroups;
        studyProgramToChange.HowManySemiGroupsAreInAGroup = howManySemiGroupsAreInAGroup;

        studyProgramToChange.Name = name;
        studyProgramToChange.Year = year;
        studyProgramToChange.WeeksInASemester = weeksInASemester;
        studyProgramToChange.NumberOfSemiGroups = numberOfSemiGroups;
        studyProgramToChange.HowManySemiGroupsAreInAGroup = howManySemiGroupsAreInAGroup;
        
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
        
        var groupsToDelete = await _dbContext.StudyYearGroups.Where(s => s.StudyProgramYearId == id)
            .ToListAsync();

        _dbContext.StudyYearGroups.RemoveRange(groupsToDelete);

        _dbContext.StudyPrograms.Remove(studyProgramToDelete);
        await _dbContext.SaveChangesAsync();

        return response;
    }

    public async Task SaveStudyProgram()
    {
        await _dbContext.SaveChangesAsync();
    }
}