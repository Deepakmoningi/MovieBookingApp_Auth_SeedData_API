using System.ComponentModel.DataAnnotations;

namespace AuthenticationAPI.DTOS
{
    public class RegisterRequest
    {
        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required,EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string LoginId { get; set; } = string.Empty;

        [Required]
        public string Role { get; set; }= string.Empty;

        [Required]
        public string ContactNumber { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        [Required,Compare(nameof(Password),ErrorMessage = "Password and confirm password are not matching.")]
        public string ConfirmPassword { get; set; } = string.Empty;
        
    }
}
