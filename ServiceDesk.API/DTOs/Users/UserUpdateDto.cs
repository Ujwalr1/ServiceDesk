using System.ComponentModel.DataAnnotations;

namespace ServiceDesk.API.DTOs.Users
{
    public class UserUpdateDto
    {
        [Required]
        [MaxLength(100)]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(150)]
        public string Email { get; set; }

        [Required]
        [MaxLength(50)]
        public string Role { get; set; }

        public bool IsActive { get; set; }
    }
}
