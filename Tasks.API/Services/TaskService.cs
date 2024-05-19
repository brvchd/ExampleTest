using Tasks.API.DTOs.Requests;
using Tasks.API.Exceptions;
using Tasks.API.Repositories.Abstractions;

namespace Tasks.API.Services;

public class TaskService : ITaskService
{
    private readonly IProjectRepository _projectRepository;
    private readonly ITaskTypeRepository _taskTypeRepository;
    private readonly ITeamMemberRepository _teamMemberRepository;
    private readonly ITaskRepository _taskRepository;

    public TaskService(
        IProjectRepository projectRepository,
        ITaskTypeRepository taskTypeRepository,
        ITeamMemberRepository teamMemberRepository,
        ITaskRepository taskRepository)
    {
        _projectRepository = projectRepository;
        _taskTypeRepository = taskTypeRepository;
        _teamMemberRepository = teamMemberRepository;
        _taskRepository = taskRepository;
    }

    public async Task<int> CreateNewTask(CreateNewTaskDto createNewTaskDto)
    {
        if (!await _projectRepository.ProjectExists(createNewTaskDto.IdTeam))
        {
            throw new ProjectNotFound(createNewTaskDto.IdTeam);
        }
        
        if (!await _taskTypeRepository.TaskTypeExists(createNewTaskDto.IdTaskType))
        {
            throw new TaskTypeNotFound(createNewTaskDto.IdTaskType);
        }
        
        if (!await _teamMemberRepository.TeamMemberExists(createNewTaskDto.IdCreator))
        {
            throw new TeamMemberNotFound(createNewTaskDto.IdCreator);
        }
        
        if (!await _teamMemberRepository.TeamMemberExists(createNewTaskDto.IdAssignedTo))
        {
            throw new TeamMemberNotFound(createNewTaskDto.IdAssignedTo);
        }

        var taskId = await _taskRepository.CreateNewTaskAsync(createNewTaskDto);
        return taskId;
    }
}