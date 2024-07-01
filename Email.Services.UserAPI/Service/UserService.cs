using Microsoft.AspNetCore.Identity;
using Email.Services.UserAPI.Data;
using Email.Services.UserAPI.Models;
using Email.Services.UserAPI.Models.DTO;
using Email.Services.UserAPI.Service.IService;

namespace Email.Services.UserAPI.Service
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
       // private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJWTTokenGenerator _jwtTokenGenerator;
        public UserService(AppDbContext db,UserManager<ApplicationUser> userManager,
             IJWTTokenGenerator jWTTokenGenerator)
        {
            _db = db;
            _userManager = userManager;
            //_roleManager = roleManager;
            _jwtTokenGenerator = jWTTokenGenerator;
        }

        //public async Task<bool> AssignRole(string email, string roleName)
        //{
        //    var user = _db.ApplicationUsers.FirstOrDefault(u => u.Email.ToLower() == email.ToLower());
        //    if (user != null)
        //    {
        //        if (!_roleManager.RoleExistsAsync(roleName).GetAwaiter().GetResult())
        //        {
        //            _roleManager.CreateAsync(new IdentityRole(roleName)).GetAwaiter().GetResult();

        //        }
        //        await _userManager.AddToRoleAsync(user, roleName);
        //        return true;
        //    }
        //    return false;
        //}

        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(u => u.UserName.ToLower() == loginRequestDto.UserName.ToLower());
            bool isValid = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);
            if (user == null || isValid == false)
            {
                return new LoginResponseDto()
                {
                    User = null,
                    Token = ""
                };
            }
            //var roles = await _userManager.GetRolesAsync(user);
            var token = _jwtTokenGenerator.GenerateToken(user);
            UserDto userDto = new()
            {
                Email = user.Email,
                Id = user.Id,
                Name = user.Name,
                PhoneNumber = user.PhoneNumber

            };
            LoginResponseDto loginResponseDto = new LoginResponseDto()
            {
                User = userDto,
                Token = token
            };
            return loginResponseDto;
        }

        public async Task<string> Register(RegistrationRequestDto registrationRequestDto)
        {
            ApplicationUser user = new()
            {
                UserName = registrationRequestDto.Email,
                Email = registrationRequestDto.Email,
                NormalizedEmail = registrationRequestDto.Email.ToUpper(),
                Name = registrationRequestDto.Name,
                PhoneNumber = registrationRequestDto.PhoneNumber
            };
            try
            {
                var result = await _userManager.CreateAsync(user, registrationRequestDto.Password);
                if (result.Succeeded)
                {
                    var userToReturn = _db.ApplicationUsers.First(u => u.UserName == registrationRequestDto.Email);
                    UserDto userDto = new()
                    {
                        Email = userToReturn.Email,
                        Id = userToReturn.Id,
                        Name = userToReturn.Name,
                        PhoneNumber = userToReturn.PhoneNumber
                    };
                    return "";
                }
                else
                {
                    return result.Errors.FirstOrDefault().Description;
                }
            }
            catch (Exception ex)
            {
            }
            return "Error Encountered";
        }
    }
}
