using ScheduleFaculty.Core.Entities;
using ScheduleFaculty.Core.Utils;

namespace ScheduleFaculty.Core.Services.Abstractions;

public interface IStatusRepository
{
    Task<ActionResponse<Status>> GetStatus(Guid id);

    Task<ActionResponse<Status>> GetActiveStatus();

    Task<ActionResponse<List<Status>>> GetAllStatuses();

    Task<ActionResponse<Status>> CreateStatus(string name);

    Task<ActionResponse<Status>> EditStatus(Guid id, string name, bool isActive, int semester);

    Task<ActionResponse> DeleteStatus(Guid id);
}