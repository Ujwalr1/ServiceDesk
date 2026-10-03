using Microsoft.AspNetCore.Mvc;
using ServiceDesk.API.DTOs.Users;
using ServiceDesk.API.Services.Interfaces;

namespace ServiceDesk.API.Controllers
{
    /// <summary>
    /// Provides operations for managing ServiceDesk users.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Gets all ServiceDesk users.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(
            StatusCodes.Status200OK,
            Type = typeof(IEnumerable<UserResponseDto>))]
        public async Task<ActionResult<IEnumerable<UserResponseDto>>> GetAll()
        {
            var users = await _userService.GetAllAsync();

            return Ok(users);
        }

        /// <summary>
        /// Gets a ServiceDesk user by their ID.
        /// </summary>
        /// <param name="id">The user ID.</param>s
        [HttpGet("{id}")]
        [ProducesResponseType(
            StatusCodes.Status200OK,
            Type = typeof(UserResponseDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserResponseDto>> GetById(int id)
        {
            var user = await _userService.GetByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        /// <summary>
        /// Creates a new ServiceDesk user.
        /// </summary>
        /// <param name="dto">User creation information.</param>
        [HttpPost]
        [ProducesResponseType(
            StatusCodes.Status201Created,
            Type = typeof(UserResponseDto))]
        [ProducesResponseType(
            StatusCodes.Status400BadRequest,
            Type = typeof(ProblemDetails))]
        public async Task<ActionResult<UserResponseDto>> Create(
            UserCreateDto dto)
        {
            var user = await _userService.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = user.Id },
                user);
        }

        /// <summary>
        /// Updates an existing ServiceDesk user.
        /// </summary>
        /// <param name="id">The user ID.</param>
        /// <param name="dto">Updated user information.</param>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(
            StatusCodes.Status400BadRequest,
            Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(
            int id,
            UserUpdateDto dto)
        {
            var updated = await _userService.UpdateAsync(id, dto);

            if (!updated)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
