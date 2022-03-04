using RateSetter.Models;

namespace RateSetter.Services.Interfaces
{
    public interface IUserRepository
    {
        bool IsMatch(User newUser, User existingUser);
        bool CreateUser(User newUser);
    }
}