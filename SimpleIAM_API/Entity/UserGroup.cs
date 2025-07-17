
using System.Text.RegularExpressions;

namespace SimpleIAM_API.Entity
{
    public class UserGroup
    {
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public int GroupId { get; set; }
        public Group Group { get; set; } = null!;
    }

}
