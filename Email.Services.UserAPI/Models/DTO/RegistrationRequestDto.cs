using System.ComponentModel.DataAnnotations;

namespace Email.Services.UserAPI.Models.DTO
{
    public class RegistrationRequestDto
    {
        [EmailAddress]
        public string Email { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        
    }
}
