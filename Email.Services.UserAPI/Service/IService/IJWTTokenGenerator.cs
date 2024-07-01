using Email.Services.UserAPI.Models;

namespace Email.Services.UserAPI.Service.IService
{
    public interface IJWTTokenGenerator
    {
        string GenerateToken(ApplicationUser applicationUser);
    }
}
