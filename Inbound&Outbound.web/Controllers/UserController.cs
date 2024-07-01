using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using User.Web.Models;
using User.Web.Service.IService;
using User.Web.Utility;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace User.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly ITokenProvider _tokenProvider;
        public UserController(IUserService userService, ITokenProvider tokenProvider)
        {
            _userService = userService;
            _tokenProvider = tokenProvider;
        }
        /// <summary>
        /// Login user
        /// </summary>
        /// <returns>It returns user to be logined </returns>
        [HttpGet]
        public IActionResult Login()
        {
            LoginRequestDto loginRequestDto = new();
            return View(loginRequestDto);
        }

        /// <summary>
        /// User logined
        /// </summary>
        /// <param name="loginRequestDto"></param>
        /// <returns>It returns user to be logined </returns>
        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDto loginRequestDto)
        {
            
            ResponseDto response = await _userService.LoginAsync(loginRequestDto);

            if (response != null && response.IsSuccess)
            {
                LoginResponseDto loginResponseDto = JsonConvert.DeserializeObject<LoginResponseDto>(Convert.ToString(response.Result));
                
                await SignInUser(loginResponseDto);
               _tokenProvider.SetToken(loginResponseDto.Token);
                return RedirectToAction("Index", "Home");
               
               
            }
            else
            {
                TempData["error"] = response.Message;
                return View(loginRequestDto);
            }
        }

        /// <summary>
        /// Register new user
        /// </summary>
        /// <returns>it returns user to be registered</returns>
        [HttpGet]
        public IActionResult Register()
        {
            var roleList = new List<SelectListItem>()
            {
                new SelectListItem{Text=Sd.RoleAdmin,Value=Sd.RoleAdmin},
                new SelectListItem{Text=Sd.RoleCustomer,Value=Sd.RoleCustomer},
            };
            ViewBag.RoleList = roleList;
            return View();
        }

        /// <summary>
        /// New user registered
        /// </summary>
        /// <param name="registrationRequestDto"></param>
        /// <returns>Returns new user to be registered</returns>
        [HttpPost]
        public async Task<IActionResult> Register(RegistrationRequestDto registrationRequestDto)
        {
            ResponseDto result = await _userService.RegisterAsync(registrationRequestDto);
            ResponseDto assignRole;
            if (result != null && result.IsSuccess)
            {
                if (string.IsNullOrEmpty(registrationRequestDto.Role))
                {
                    registrationRequestDto.Role = Sd.RoleCustomer;
                }
                assignRole = await _userService.AssignRoleAsync(registrationRequestDto);
                if (assignRole != null && assignRole.IsSuccess)
                {
                    TempData["Success"] = "Registration Successful";
                    return RedirectToAction(nameof(Login));
                }
            }
            else
            {
                TempData["error"] = result.Message;
            }
            var roleList = new List<SelectListItem>()
            {
                new SelectListItem{Text=Sd.RoleAdmin,Value=Sd.RoleAdmin},
                new SelectListItem{Text=Sd.RoleCustomer,Value=Sd.RoleCustomer},
            };
            ViewBag.RoleList = roleList;
            return View();
        }

       /// <summary>
       /// Logout
       /// </summary>
       /// <returns>Logout</returns>
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            _tokenProvider.ClearToken();
            return RedirectToAction("Login", "User");
        }
        private async Task SignInUser(LoginResponseDto model)
        {
            var handler = new JwtSecurityTokenHandler();

            var jwt = handler.ReadJwtToken(model.Token);
           
           
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);

           
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Email, jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub, jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Name, jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Name).Value));
            identity.AddClaim(new Claim(ClaimTypes.Name, jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));
            identity.AddClaim(new Claim(ClaimTypes.Role, jwt.Claims.FirstOrDefault(u => u.Type == "role").Value));
           
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }
    }
}
