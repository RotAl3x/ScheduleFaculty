using System.Collections.Specialized;
using ScheduleFaculty.Core.Entities;
using ScheduleFaculty.Core.Utils;

namespace ScheduleFaculty.Core.Services.Abstractions;

public interface IStudyProgramRepository
{
    Task<ActionResponse<StudyProgram>> GetStudyProgram(Guid id);
    Task<ActionResponse<List<StudyProgram>>> GetAllStudyProgramS();

    Task<ActionResponse<StudyProgram>> CreateStudyProgram(string name, int year, int weeksInASemester,
        int numberOfSemiGroups, int howManySemiGroupsAreInAGroup);
    Task<ActionResponse<StudyProgram>> EditStudyProgram(Guid id, string name, int year,int weeksInASemester,int numberOfSemiGroups,int howManySemiGroupsAreInAGroup);
    Task<ActionResponse> DeleteStudyProgram(Guid id);
    
    Task SaveStudyProgram();
}