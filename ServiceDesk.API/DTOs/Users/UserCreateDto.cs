using System.ComponentModel.DataAnnotations;

namespace ServiceDesk.API.DTOs.Users
{
    public class UserCreateDto
    {
        [Required]
        [MaxLength(100)]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(150)]
        public string Email { get; set; }

        [Required]
        [MinLength(8)]
        public string Password { get; set; }

        [Required]
        [MaxLength(50)]
        public string Role { get; set; }
    }
}
