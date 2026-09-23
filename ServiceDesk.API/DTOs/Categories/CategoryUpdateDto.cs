using System.ComponentModel.DataAnnotations;

namespace ServiceDesk.API.DTOs.Categories
{
    public class CategoryUpdateDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        public bool IsActive { get; set; }
    }
}
