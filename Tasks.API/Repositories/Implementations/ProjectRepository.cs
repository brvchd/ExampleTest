using Microsoft.Data.SqlClient;
using Tasks.API.Repositories.Abstractions;

namespace Tasks.API.Repositories.Implementations;

public class ProjectRepository : IProjectRepository
{
    private readonly string _connectionString;

    public ProjectRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new ArgumentNullException();
    }
    
    public async Task<bool> ProjectExists(int projectId)
    {
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        
        var command = new SqlCommand("SELECT COUNT(1) FROM Project WHERE IdProject = @IdProject");
        command.Connection = connection;
        command.Parameters.AddWithValue("@IdProject", projectId);
        
        var count = (int)(await command.ExecuteScalarAsync() ?? throw new InvalidOperationException());

        return count > 0;
    }
}