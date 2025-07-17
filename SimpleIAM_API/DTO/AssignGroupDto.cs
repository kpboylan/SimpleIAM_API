namespace SimpleIAM_API.DTO
{
    public class AssignGroupDto
    {
        public string Email { get; set; } = string.Empty;

        public int GroupId { get; set; }
        public string GroupName { get; set; } = string.Empty;
    }
}
