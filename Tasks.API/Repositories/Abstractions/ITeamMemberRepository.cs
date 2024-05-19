using Tasks.API.Model;

namespace Tasks.API.Repositories.Abstractions;

public interface ITeamMemberRepository
{
    Task<TeamMember?> GetTeamMember(int memberId);
    Task<bool> TeamMemberExists(int memberId);
}