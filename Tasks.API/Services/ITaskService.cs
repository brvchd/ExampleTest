using Tasks.API.DTOs.Requests;

namespace Tasks.API.Services;

public interface ITaskService
{
    Task<int> CreateNewTask(CreateNewTaskDto createNewTaskDto);
}