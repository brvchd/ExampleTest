using Microsoft.Data.SqlClient;
using Tasks.API.DTOs.Requests;
using Tasks.API.Model;
using Tasks.API.Repositories.Abstractions;

namespace Tasks.API.Repositories.Implementations;

public class TaskRepository : ITaskRepository
{
    private readonly string _connectionString;

    public TaskRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new ArgumentNullException();
    }

    public async Task<List<TeamMemberTask>> GetTasksByCreatorIdAsync(int creatorId)
    {
        var tasks = new List<TeamMemberTask>();
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        var command = new SqlCommand("""
                                     SELECT t.Name as TaskName,
                                            t.Description as TaskDescription,
                                            t.Deadline as TaskDeadline,
                                            p.Name as ProjectName,
                                            tt.Name as TaskType
                                     FROM TASK as t
                                     INNER JOIN Project as p on p.IdProject = t.IdProject
                                     INNER JOIN TaskType as tt on tt.IdTaskType = t.IdTaskType
                                     WHERE t.IdCreator = @IdCreator
                                     Order by t.Deadline DESC
                                     """, connection);
        command.Parameters.AddWithValue("@IdCreator", creatorId);
        await using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            tasks.Add(new TeamMemberTask
            {
                Name = reader.GetString(0),
                Description = reader.GetString(1),
                Deadline = reader.GetDateTime(2),
                ProjectName = reader.GetString(3),
                TaskTypeName = reader.GetString(4)
            });
        }

        return tasks;
    }

    public async Task<List<TeamMemberTask>> GetTasksByAssignedToIdAsync(int assignedToId)
    {
        var tasks = new List<TeamMemberTask>();
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        var command = new SqlCommand("""
                                     SELECT t.Name as TaskName,
                                            t.Description as TaskDescription,
                                            t.Deadline as TaskDeadline,
                                            p.Name as ProjectName,
                                            tt.Name as TaskType
                                     FROM TASK as t
                                     INNER JOIN Project as p on p.IdProject = t.IdProject
                                     INNER JOIN TaskType as tt on tt.IdTaskType = t.IdTaskType
                                     WHERE t.IdAssignedTo = @IdAssignedTo
                                     Order by t.Deadline DESC
                                     """, connection);
        command.Parameters.AddWithValue("@IdAssignedTo", assignedToId);
        await using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            tasks.Add(new TeamMemberTask
            {
                Name = reader.GetString(0),
                Description = reader.GetString(1),
                Deadline = reader.GetDateTime(2),
                ProjectName = reader.GetString(3),
                TaskTypeName = reader.GetString(4)
            });
        }

        return tasks;
    }

    public async Task<int> CreateNewTaskAsync(CreateNewTaskDto createNewTaskDto)
    {
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        await using var transaction = connection.BeginTransaction();
        
        try
        {
            var command =
                new SqlCommand("""
                               INSERT INTO Task(Name, Description, Deadline, IdProject, IdTaskType, IdAssignedTo, IdCreator)
                               VALUES (@Name, @Description, @Deadline, @IdProject, @IdTaskType,@IdAssignedTo, @IdCreator)
                               SELECT SCOPE_IDENTITY()
                               """);
            
            command.Transaction = transaction;
            command.Connection = connection;
            
            command.Parameters.AddWithValue("@Name", createNewTaskDto.Name);
            command.Parameters.AddWithValue("@Description", createNewTaskDto.Description);
            command.Parameters.AddWithValue("@Deadline", createNewTaskDto.Deadline);
            command.Parameters.AddWithValue("@IdProject", createNewTaskDto.IdTeam);
            command.Parameters.AddWithValue("@IdTaskType", createNewTaskDto.IdTaskType);
            command.Parameters.AddWithValue("@IdAssignedTo", createNewTaskDto.IdAssignedTo);
            command.Parameters.AddWithValue("@IdCreator", createNewTaskDto.IdCreator);

            var taskId = (decimal)(await command.ExecuteScalarAsync() ?? throw new InvalidOperationException());
            await transaction.CommitAsync();
            return decimal.ToInt32(taskId);
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}