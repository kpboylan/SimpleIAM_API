namespace SimpleIAM_API.Entity
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public List<UserGroup> Groups { get; set; } = new List<UserGroup>();
    }

}
