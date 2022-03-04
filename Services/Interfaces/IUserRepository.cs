using RateSetter.Models;
using RateSetter.ViewModels;

namespace RateSetter.Services.Interfaces
{
    public interface IUserRepository
    {
        bool IsMatch(UserAddressViewModel model);
        bool CreateUser(UserAddressViewModel model);

        public UserAddressViewModel UserAddressViewModel();
    }
}