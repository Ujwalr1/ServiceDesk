namespace ServiceDesk.API.DTOs.Categories
{
    public class CategoryUpdateDto
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }
    }
}
