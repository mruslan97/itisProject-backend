using Microsoft.AspNetCore.Identity;

namespace AspNetCore.Models.Entities
{
    // Add profile data for application users by adding properties to this class
    public class AppUser : IdentityUser
    {
        // Extended Properties
        public string FirstName { get; set; }

        public string LastName { get; set; }
        public bool IsAdmin { get; set; }
    }
}