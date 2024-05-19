using System.ComponentModel.DataAnnotations;

namespace Tasks.API.DTOs.Requests;

public class CreateNewTaskDto
{
    [MaxLength(100)] public string Name { get; set; } = string.Empty;
    [MaxLength(100)] public string Description { get; set; } = string.Empty;
    public DateTime Deadline { get; set; }
    public int IdTeam { get; set; }
    public int IdTaskType { get; set; }
    public int IdAssignedTo { get; set; }
    public int IdCreator { get; set; }
}