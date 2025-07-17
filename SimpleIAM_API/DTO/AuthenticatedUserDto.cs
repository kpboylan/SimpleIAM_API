namespace SimpleIAM_API.DTO
{
    public class AuthenticatedUserDto
    {
        public string Message { get; set; } = "Authentication successful.";
        public int UserId { get; set; }
        public string Email { get; set; } = string.Empty;
        public List<string> Groups { get; set; } = new();
    }
}
