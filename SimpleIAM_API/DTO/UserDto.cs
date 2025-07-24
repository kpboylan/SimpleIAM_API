namespace SimpleIAM_API.DTO
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public List<string> Groups { get; set; } = new();

        public string GroupName { get; set; } = string.Empty;
    }
}
