using System.ComponentModel.DataAnnotations;

namespace AuthenticationAPI.DTOS
{
    public class LoginRequest
    {
        [Required]
        public string LoginId { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
