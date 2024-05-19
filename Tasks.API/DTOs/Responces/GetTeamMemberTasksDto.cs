using Tasks.API.Model;

namespace Tasks.API.DTOs.Responces;

public class GetTeamMemberTasksDto
{
    public TeamMember TeamMember { get; set; } = new();
    public List<TeamMemberTask> CreatedTasks { get; set; } = [];
    public List<TeamMemberTask> AssignedToTasks { get; set; } = [];

}