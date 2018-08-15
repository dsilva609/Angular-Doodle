using Microsoft.AspNetCore.Identity;

namespace Angular_Doodle.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string DisplayName { get; set; }
    }
}