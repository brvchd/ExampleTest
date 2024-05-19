namespace Tasks.API.Repositories.Abstractions;

public interface IProjectRepository
{
    Task<bool> ProjectExists(int projectId);
}