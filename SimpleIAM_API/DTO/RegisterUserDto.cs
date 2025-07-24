namespace SimpleIAM_API.DTO
{
    public class RegisterUserDto
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string GroupName { get; set; } = string.Empty;
    }
}
