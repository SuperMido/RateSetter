using RateSetter.Models;
using RateSetter.Services.Interfaces;

namespace RateSetter.Services
{
    public class UserRepository : IUserRepository
    {
        public bool IsMatch(User newUser, User existingUser)
        {
            throw new System.NotImplementedException();
        }
    }
}