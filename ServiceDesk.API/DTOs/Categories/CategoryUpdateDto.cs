using System.ComponentModel.DataAnnotations;

namespace ServiceDesk.API.DTOs.Categories
{
    public class CategoryUpdateDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;

        public bool IsActive { get; set; }
    }
}
