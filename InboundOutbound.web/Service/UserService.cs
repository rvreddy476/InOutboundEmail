using InboundOutbound.web.Models;
using InboundOutbound.web.Service.IService;
using InboundOutbound.web.Utility;

namespace InboundOutbound.web.Service
{
    public class UserService : IUserService
    {
        private readonly IBaseService _baseService;
        public UserService(IBaseService baseService)
        {
            _baseService = baseService;
        }
        /// <summary>
        /// Method to handle user login
        /// </summary>
        /// <param name="loginRequestDto"></param>
        /// <returns></returns>
        public async Task<ResponseDto?> LoginAsync(LoginRequestDto loginRequestDto)
        {
            // Call the base service to send a POST request for login
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = Sd.ApiType.POST,                      // Set the API type to POST
                Data = loginRequestDto,                        // Pass the login request data
                Url = Sd.UserAPIBase + "/api/user/login"       // Set the URL for the login API endpoint
            });
        }

        /// <summary>
        /// Method to handle user registration
        /// </summary>
        /// <param name="registrationRequestDto"></param>
        /// <returns></returns>
        public async Task<ResponseDto?> RegisterAsync(RegistrationRequestDto registrationRequestDto)
        {
            // Call the base service to send a POST request for registration
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = Sd.ApiType.POST,                     // Set the API type to POST
                Data = registrationRequestDto,                 // Pass the registration request data
                Url = Sd.UserAPIBase + "/api/user/register"    // Set the URL for the registration API endpoint
            });
        }
    }
}
