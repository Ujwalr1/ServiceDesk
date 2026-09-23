using ServiceDesk.API.DTOs.Users;

namespace ServiceDesk.API.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserResponseDto>> GetAllAsync();

        Task<UserResponseDto?> GetByIdAsync(int id);

        Task<UserResponseDto> CreateAsync(UserCreateDto dto);

        Task<bool> UpdateAsync(int id, UserUpdateDto dto);

        //Task<bool> DeleteAsync(int id);
    }
}
