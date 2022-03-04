using System.Threading.Tasks;
using RateSetter.Models;
using RateSetter.ViewModels;

namespace RateSetter.Services.Interfaces
{
    public interface IUserRepository
    {
        bool IsMatch(User newUser, User existingUser);
        bool CreateUser(UserAddressViewModel model);

        public UserAddressViewModel UserAddressViewModel();
    }
}