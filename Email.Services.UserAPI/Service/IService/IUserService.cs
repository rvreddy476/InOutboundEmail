using Email.Services.UserAPI.Models.DTO;

namespace Email.Services.UserAPI.Service.IService
{
    public interface IUserService
    {
        Task<string> Register(RegistrationRequestDto registrationRequestDto);
        Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);
       // Task<bool> AssignRole(string email, string roleName);
    }
}
