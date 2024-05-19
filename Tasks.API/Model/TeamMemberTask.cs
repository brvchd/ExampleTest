namespace Tasks.API.Model;

public class TeamMemberTask
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime Deadline { get; set; }
    public string ProjectName { get; set; } = string.Empty;
    public string TaskTypeName { get; set; } = string.Empty;
}