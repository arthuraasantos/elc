using Core.Entities.Users;

namespace Core.Factories
{
    public static class UserFactory
    {
        public static AppUser Create(string name, string email)
        {
            return new AppUser(name, email)
            {
            };
        }
    }
}
