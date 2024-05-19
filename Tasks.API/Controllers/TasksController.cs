using Microsoft.AspNetCore.Mvc;
using Tasks.API.DTOs.Requests;
using Tasks.API.DTOs.Responces;
using Tasks.API.Exceptions;
using Tasks.API.Repositories.Abstractions;
using Tasks.API.Services;

namespace Tasks.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly ITaskRepository _taskRepository;
    private readonly ITeamMemberRepository _teamMemberRepository;
    private readonly ITaskService _taskService;

    public TasksController(ITaskRepository taskRepository, ITeamMemberRepository teamMemberRepository,
        ITaskService taskService)
    {
        _taskRepository = taskRepository;
        _teamMemberRepository = teamMemberRepository;
        _taskService = taskService;
    }
    
    // Example using repositories only
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetTeamMemberTasks(int id)
    {
        var teamMember = await _teamMemberRepository.GetTeamMember(id);

        if (teamMember is null)
            return NotFound($"Team member with {id} is not found");

        var assignedTasks = await _taskRepository.GetTasksByAssignedToIdAsync(id);
        var createdTasks = await _taskRepository.GetTasksByCreatorIdAsync(id);

        var teamMemberTasksDto = new GetTeamMemberTasksDto
        {
            TeamMember = teamMember,
            AssignedToTasks = assignedTasks,
            CreatedTasks = createdTasks
        };

        return Ok(teamMemberTasksDto);
    }

    // Example using services
    [HttpPost]
    public async Task<IActionResult> CreateNewTask(CreateNewTaskDto createNewTaskDto)
    {
        try
        {
            var createdTaskId = await _taskService.CreateNewTask(createNewTaskDto);
            
            // Although, there is not such endpoint
            return Created("api/tasks/id", new
            {
                Id = createdTaskId
            });
        }
        catch (NotFoundException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}