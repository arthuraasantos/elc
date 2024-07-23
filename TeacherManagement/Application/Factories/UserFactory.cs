using Core.Entities.Users;

namespace Core.Factories
{
    public static class UserFactory
    {
        public static AppUser Create(string name, string email)
        {
            var treatedName = name.Replace(".", string.Empty);

            return new AppUser(treatedName, email)
            {
            };
        }
    }
}
