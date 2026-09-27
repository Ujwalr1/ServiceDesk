using Microsoft.EntityFrameworkCore;
using ServiceDesk.API.DTOs.Users;
using ServiceDesk.API.Services.Interfaces;
using ServiceDesk.Data.Data;
using ServiceDesk.Data.Entities;

namespace ServiceDesk.API.Services
{
    public class UserService : IUserService
    {
        private readonly ServiceDeskDbContext _context;

        public UserService(ServiceDeskDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserResponseDto>> GetAllAsync()
        {
            return await _context.Users
                .Select(u => new UserResponseDto
                {
                    Id = u.Id,
                    FullName = u.FullName,
                    Email = u.Email,
                    Role = u.Role,
                    IsActive = u.IsActive,
                    CreatedAt = u.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<UserResponseDto?> GetByIdAsync(int id)
        {
            return await _context.Users
                .Where(u => u.Id == id)
                .Select(u => new UserResponseDto
                {
                    Id = u.Id,
                    FullName = u.FullName,
                    Email = u.Email,
                    Role = u.Role,
                    IsActive = u.IsActive,
                    CreatedAt = u.CreatedAt
                })
                .FirstOrDefaultAsync();
        }

        public async Task<UserResponseDto> CreateAsync(UserCreateDto dto)
        {
            // Validate full name
            if (string.IsNullOrWhiteSpace(dto.FullName))
            {
                throw new ArgumentException(
                    "Full name cannot be empty.");
            }

            // Validate email
            if (string.IsNullOrWhiteSpace(dto.Email))
            {
                throw new ArgumentException(
                    "Email cannot be empty.");
            }

            // Validate role
            if (string.IsNullOrWhiteSpace(dto.Role))
            {
                throw new ArgumentException(
                    "Role cannot be empty.");
            }

            var email = dto.Email.Trim();

            // Check for duplicate email
            var emailExists = await _context.Users
                .AnyAsync(u => u.Email == email);

            if (emailExists)
            {
                throw new ArgumentException(
                    "A user with this email already exists.");
            }

            var user = new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                Role = dto.Role.Trim(),
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
            };

            _context.Users.Add(user);

            await _context.SaveChangesAsync();

            return new UserResponseDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt
            };
        }

        public async Task<bool> UpdateAsync(int id, UserUpdateDto dto)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return false;
            }

            // Validate full name
            if (string.IsNullOrWhiteSpace(dto.FullName))
            {
                throw new ArgumentException(
                    "Full name cannot be empty.");
            }

            // Validate email
            if (string.IsNullOrWhiteSpace(dto.Email))
            {
                throw new ArgumentException(
                    "Email cannot be empty.");
            }

            // Validate role
            if (string.IsNullOrWhiteSpace(dto.Role))
            {
                throw new ArgumentException(
                    "Role cannot be empty.");
            }

            var email = dto.Email.Trim();

            // Check whether another user already uses this email
            var emailExists = await _context.Users
                .AnyAsync(u =>
                    u.Id != id &&
                    u.Email == email);

            if (emailExists)
            {
                throw new ArgumentException(
                    "A user with this email already exists.");
            }

            user.FullName = dto.FullName.Trim();
            user.Email = dto.Email;
            user.Role = dto.Role.Trim();
            user.IsActive = dto.IsActive;

            await _context.SaveChangesAsync();

            return true;
        }

        //public async Task<bool> DeleteAsync(int id)
        //{
        //    var user = await _context.Users.FindAsync(id);

        //    if (user == null)
        //    {
        //        return false;
        //    }

        //    _context.Users.Remove(user);

        //    await _context.SaveChangesAsync();

        //    return true;
        //}
    }
}
