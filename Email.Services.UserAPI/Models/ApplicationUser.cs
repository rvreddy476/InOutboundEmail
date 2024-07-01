using Microsoft.AspNetCore.Identity;

namespace Email.Services.UserAPI.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string Name { get; set; }
    }
}
