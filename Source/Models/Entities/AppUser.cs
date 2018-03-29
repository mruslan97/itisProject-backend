using System.ComponentModel;
using Microsoft.AspNetCore.Identity;

namespace AspNetCore.Models.Entities
{
    public enum Roles 
    {
        Customer,
        Support,
        Admin
    }
    // Add profile data for application users by adding properties to this class
    public class AppUser : IdentityUser
    {
        // Extended Properties
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Roles Role { get; set; }
    }
}