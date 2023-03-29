using ScheduleFaculty.Core.Entities;
using ScheduleFaculty.Core.Utils;

namespace ScheduleFaculty.Core.Services.Abstractions;

public interface IStudyYearGroupRepository
{
    Task<ActionResponse<List<StudyYearGroup>>> GetStudyYearGroup(Guid studyProgramId);
    Task<ActionResponse<List<List<StudyYearGroup>>>> GetAllStudyYearGroups();
    Task<ActionResponse<TotalGroups>> GetStudyYearNumberOfGroups(Guid studyProgramId);
    Task<ActionResponse<List<TotalGroups>>> GetAllStudyYearNumberOfGroups();
    Task<ActionResponse<List<StudyYearGroup>>> CreateStudyYearGroup(Guid studyProgramId, int numberOfSemiGroups, int howManySemiGroupsAreInAGroup);
    Task<ActionResponse<List<StudyYearGroup>>> EditStudyYearGroup(Guid studyProgramId, int numberOfSemiGroups, int howManySemiGroupsAreInAGroup);
    Task<ActionResponse> DeleteStudyYearGroup(Guid studyProgramId);
    
    Task SaveStudyYearGroup();
}