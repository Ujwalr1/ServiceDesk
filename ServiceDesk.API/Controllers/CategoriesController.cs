using Microsoft.AspNetCore.Mvc;
using ServiceDesk.API.DTOs.Categories;
using ServiceDesk.API.Services.Interfaces;

namespace ServiceDesk.API.Controllers
{
    /// <summary>
    /// Provides operations for managing service desk categories.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        /// <summary>
        /// Gets all categories, including inactive categories.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(
            StatusCodes.Status200OK,
            Type = typeof(IEnumerable<CategoryResponseDto>))]
        public async Task<ActionResult<IEnumerable<CategoryResponseDto>>> GetAll()
        {
            var categories = await _categoryService.GetAllAsync();

            return Ok(categories);
        }

        /// <summary>
        /// Gets all active categories.
        /// </summary>
        [HttpGet("active")]
        [ProducesResponseType(
            StatusCodes.Status200OK,
            Type = typeof(IEnumerable<CategoryResponseDto>))]
        public async Task<ActionResult<IEnumerable<CategoryResponseDto>>> GetActive()
        {
            var categories = await _categoryService.GetActiveAsync();

            return Ok(categories);
        }

        /// <summary>
        /// Gets a category by its ID.
        /// </summary>
        /// <param name="id">The category ID.</param>s
        [HttpGet("{id}")]
        [ProducesResponseType(
            StatusCodes.Status200OK,
            Type = typeof(CategoryResponseDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CategoryResponseDto>> GetById(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        /// <summary>
        /// Creates a new category.
        /// </summary>
        /// <param name="dto">Category creation information.</param>
        [HttpPost]
        [ProducesResponseType(
            StatusCodes.Status201Created,
            Type = typeof(CategoryResponseDto))]
        [ProducesResponseType(
            StatusCodes.Status400BadRequest,
            Type = typeof(ProblemDetails))]
        public async Task<ActionResult<CategoryResponseDto>> Create(
            CategoryCreateDto dto)
        {
            var category = await _categoryService.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = category.Id },
                category);
        }

        /// <summary>
        /// Updates an existing category.
        /// </summary>
        /// <param name="id">The category ID.</param>
        /// <param name="dto">Updated category information.</param>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(
            StatusCodes.Status400BadRequest,
            Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(
            int id,
            CategoryUpdateDto dto)
        {
            var updated = await _categoryService.UpdateAsync(id, dto);

            if (!updated)
            {
                return NotFound();
            }

            return NoContent();
        }

        /// <summary>
        /// Deactivates an existing category.
        /// </summary>
        /// <param name="id">The category ID.</param>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _categoryService.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
