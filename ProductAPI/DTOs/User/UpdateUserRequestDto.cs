using System.ComponentModel.DataAnnotations;

namespace ProductAPI.DTOs.User
{
    public class UpdateUserRequestDto
    {
        [Required]
        [EmailAddress]
        [MaxLength(200)]
        public string Email { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? Password { get; set; }

        [Required]
        public string Role { get; set; } = string.Empty;
    }
}
