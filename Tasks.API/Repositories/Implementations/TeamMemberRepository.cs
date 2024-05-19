using Microsoft.Data.SqlClient;
using Tasks.API.Model;
using Tasks.API.Repositories.Abstractions;

namespace Tasks.API.Repositories.Implementations;

public class TeamMemberRepository : ITeamMemberRepository
{
    private readonly string _connectionString;

    public TeamMemberRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new ArgumentNullException();
    }
    
    public async Task<TeamMember?> GetTeamMember(int memberId)
    {
        TeamMember? teamMember = null;
        
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        
        var command = new SqlCommand("SELECT IdTeamMember, FirstName, LastName, Email FROM TeamMember WHERE IdTeamMember = @MemberId");
        command.Connection = connection;
        command.Parameters.AddWithValue("@MemberId", memberId);
        
        await using var reader = await command.ExecuteReaderAsync();
        
        if (await reader.ReadAsync())
        {
            teamMember = new TeamMember
            {
                IdTeamMember = reader.GetInt32(0),
                FirstName = reader.GetString(1),
                LastName = reader.GetString(2),
                Email = reader.GetString(3)
            };
        }

        return teamMember;
    }

    public async Task<bool> TeamMemberExists(int memberId)
    {
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        
        var command = new SqlCommand("SELECT COUNT(1) FROM TeamMember WHERE IdTeamMember = @IdTeamMember");
        command.Connection = connection;
        command.Parameters.AddWithValue("@IdTeamMember", memberId);
        
        var count = (int)await command.ExecuteScalarAsync();

        return count > 0;
    }
}