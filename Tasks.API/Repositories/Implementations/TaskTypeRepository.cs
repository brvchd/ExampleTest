using Microsoft.Data.SqlClient;
using Tasks.API.Repositories.Abstractions;

namespace Tasks.API.Repositories.Implementations;

public class TaskTypeRepository : ITaskTypeRepository
{
    private readonly string _connectionString;

    public TaskTypeRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new ArgumentNullException();
    }
    
    public async Task<bool> TaskTypeExists(int taskTypeId)
    {
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        
        var command = new SqlCommand("SELECT COUNT(1) FROM TaskType WHERE IdTaskType = @TaskTypeId");
        command.Connection = connection;
        command.Parameters.AddWithValue("@TaskTypeId", taskTypeId);
        
        var count = (int)(await command.ExecuteScalarAsync() ?? throw new InvalidOperationException());

        return count > 0;
    }
}