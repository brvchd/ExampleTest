namespace Tasks.API.Repositories.Abstractions;

public interface ITaskTypeRepository
{
    Task<bool> TaskTypeExists(int taskTypeId);
}