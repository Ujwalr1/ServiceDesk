namespace ServiceDesk.API.DTOs.Users
{
    public class UserUpdateDto
    {
        public string FullName { get; set; }

        public string Email { get; set; }

        public string Role { get; set; }

        public bool IsActive { get; set; }
    }
}
