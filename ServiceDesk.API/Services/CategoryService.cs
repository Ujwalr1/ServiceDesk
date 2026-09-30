using Microsoft.EntityFrameworkCore;
using ServiceDesk.API.DTOs.Categories;
using ServiceDesk.API.Services.Interfaces;
using ServiceDesk.Data.Data;
using ServiceDesk.Data.Entities;

namespace ServiceDesk.API.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ServiceDeskDbContext _context;
        private readonly ILogger<CategoryService> _logger;

        public CategoryService(ServiceDeskDbContext context, ILogger<CategoryService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<CategoryResponseDto>> GetAllAsync()
        {
            return await _context.Categories
                .Select(c => new CategoryResponseDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    IsActive = c.IsActive,
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<CategoryResponseDto>> GetActiveAsync()
        {
            return await _context.Categories
                .Where(c => c.IsActive)
                .Select(c => new CategoryResponseDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    IsActive = c.IsActive
                })
                .ToListAsync();
        }

        public async Task<CategoryResponseDto?> GetByIdAsync(int id)
        {
            return await _context.Categories
                .Where(c => c.Id == id)
                .Select(c => new CategoryResponseDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    IsActive= c.IsActive,
                })
                .FirstOrDefaultAsync();
        }

        public async Task<CategoryResponseDto> CreateAsync(
            CategoryCreateDto dto)
        {
            // Validate category name
            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                _logger.LogWarning(
                    "Category creation failed. Category name is empty.");

                throw new ArgumentException(
                    "Category name cannot be empty.");
            }

            // Check for duplicate category name
            var duplicateExists = await _context.Categories
                .AnyAsync(c => c.Name == dto.Name);

            if (duplicateExists)
            {
                _logger.LogWarning(
                    "Category creation failed. Category name already exists: {CategoryName}",
                    dto.Name);

                throw new ArgumentException(
                    "A category with this name already exists.");
            }

            var category = new Category
            {
                Name = dto.Name,
                Description = dto.Description,
                IsActive = true
            };

            _context.Categories.Add(category);

            await _context.SaveChangesAsync();

            _logger.LogInformation(
                    "Category created successfully. CategoryId: {CategoryId}, CategoryName: {CategoryName}",
                    category.Id,
                    category.Name);

            return new CategoryResponseDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                IsActive = category.IsActive
            };
        }

        public async Task<bool> UpdateAsync(
            int id,
            CategoryUpdateDto dto)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                _logger.LogWarning(
                    "Category update failed. Category not found. CategoryId: {CategoryId}",
                    id);

                return false;
            }

            // Validate category name
            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                _logger.LogWarning(
                    "Category update failed. Category name is empty. CategoryId: {CategoryId}",
                    id);

                throw new ArgumentException(
                    "Category name cannot be empty.");
            }

            // Check for duplicate category name
            var duplicateExists = await _context.Categories
                .AnyAsync(c =>
                    c.Id != id &&
                    c.Name == dto.Name);

            if (duplicateExists)
            {

                _logger.LogWarning(
                    "Category update failed. Category name already exists: {CategoryName}. CategoryId: {CategoryId}",
                    dto.Name,
                    id);

                throw new ArgumentException(
                    "A category with this name already exists.");
            }

            category.Name = dto.Name;
            category.Description = dto.Description;
            category.IsActive = dto.IsActive;

            await _context.SaveChangesAsync();

            _logger.LogInformation(
                "Category updated successfully. CategoryId: {CategoryId}",
                id);

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                _logger.LogWarning(
                    "Category deactivation failed. Category not found. CategoryId: {CategoryId}",
                    id);

                return false;
            }

            // Soft delete or hard delete
            category.IsActive = false; 

            //_context.Categories.Remove(category);

            await _context.SaveChangesAsync();

            _logger.LogInformation(
                "Category deactivated successfully. CategoryId: {CategoryId}",
                id);

            return true;
        }
    }
}
