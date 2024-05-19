namespace Tasks.API.Model;

public class TeamMember
{
    public int IdTeamMember { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}