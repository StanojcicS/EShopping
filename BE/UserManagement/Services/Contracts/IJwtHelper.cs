using System.Security.Claims;

namespace UserManagement.Services.Contracts
{
    public interface IJwtHelper
    {
        string GetNewJwtToken(List<Claim> claims, string userId);
    }
}
