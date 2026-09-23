using ServiceDesk.API.DTOs.Categories;

namespace ServiceDesk.API.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryResponseDto>> GetAllAsync();
        Task<IEnumerable<CategoryResponseDto>> GetActiveAsync();

        Task<CategoryResponseDto?> GetByIdAsync(int id);

        Task<CategoryResponseDto> CreateAsync(CategoryCreateDto dto);

        Task<bool> UpdateAsync(int id, CategoryUpdateDto dto);

        Task<bool> DeleteAsync(int id);
    }
}
