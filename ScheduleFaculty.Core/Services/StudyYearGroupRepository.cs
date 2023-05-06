using System.Runtime.CompilerServices;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Microsoft.EntityFrameworkCore;
using ScheduleFaculty.Core.Database;
using ScheduleFaculty.Core.Entities;
using ScheduleFaculty.Core.Migrations;
using ScheduleFaculty.Core.Services.Abstractions;
using ScheduleFaculty.Core.Utils;

namespace ScheduleFaculty.Core.Services;

public class StudyYearGroupRepository : IStudyYearGroupRepository
{
    private readonly ApplicationDbContext _dbContext;

    public StudyYearGroupRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ActionResponse<List<StudyYearGroup>>> GetStudyYearGroup(Guid studyProgramId)
    {
        var response = new ActionResponse<List<StudyYearGroup>>();
        var studyYearGroup = await _dbContext.StudyYearGroups.Where(s => s.StudyProgramYearId == studyProgramId)
            .ToListAsync();
        if (studyYearGroup.Count == 0)
        {
            response.AddError("Student program doesn't have groups");
            return response;
        }

        response.Item = studyYearGroup;
        return response;
    }

    public async Task SaveStudyYearGroup()
    {
        await _dbContext.SaveChangesAsync();
    }
}