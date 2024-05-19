using Tasks.API.DTOs.Requests;
using Tasks.API.Model;

namespace Tasks.API.Repositories.Abstractions;

public interface ITaskRepository
{
    Task<List<TeamMemberTask>> GetTasksByCreatorIdAsync(int creatorId);
    Task<List<TeamMemberTask>> GetTasksByAssignedToIdAsync(int assignedToId);
    Task<int> CreateNewTaskAsync(CreateNewTaskDto createNewTaskDto);
}