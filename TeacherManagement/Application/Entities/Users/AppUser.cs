using Microsoft.AspNetCore.Identity;

namespace Core.Entities.Users
{
    public class AppUser: IdentityUser<Guid>
    {
        [Obsolete("Used by EF", true)]
        public AppUser() { }

        public AppUser(string name, string email)
        {
            Id = Guid.NewGuid();
            UserName = name;
            Email = email;
        }
    }
}
