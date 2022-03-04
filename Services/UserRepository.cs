using System.Linq;
using System.Text.RegularExpressions;
using RateSetter.Data;
using RateSetter.Models;
using RateSetter.Services.Coordinates;
using RateSetter.Services.Interfaces;
using RateSetter.ViewModels;

namespace RateSetter.Services
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool IsMatch(User newUser, User existingUser)
        {
            // var isValidDistance = IsValidDistance(newUser, existingUser);
            throw new System.NotImplementedException();
        }

        public UserAddressViewModel UserAddressViewModel()
        {
            var userAddressViewModel = new UserAddressViewModel()
            {
                Address = new Address(),
                User = new User()
            };

            return userAddressViewModel;
        }

        public bool CreateUser(UserAddressViewModel model)
        {
            if (IsValidDistance(model.Address))
            {
                model.Address.StreetAddress = StringFormater(model.Address.StreetAddress);
                _context.Addresses.Add(model.Address);
                _context.SaveChanges();
                model.User.Name = StringFormater(model.User.Name);
                model.User.AddressId = model.Address.Id;
                _context.Users.Add(model.User);
                _context.SaveChanges();
            }
            else
            {
                return false;
            }

            return true;
        }

        private bool IsValidDistance(Address newUser)
        {
            var userAddresses = _context.Addresses.ToList();
            return !userAddresses.Any() || userAddresses.TakeWhile(a => !(GetDistance(newUser, a) <= 0.5)).Any();
        }

        private static double GetDistance(Address newUserAddress, Address existingUserAddress)
        {
            var newlat = decimal.ToDouble(newUserAddress.Latitude);
            var newLong = decimal.ToDouble(newUserAddress.Longitude);
            var existLat = decimal.ToDouble(existingUserAddress.Latitude);
            var existLong = decimal.ToDouble(existingUserAddress.Longitude);
            var distance = new Coordinates.Coordinates(newlat, newLong)
                .DistanceTo(
                    new Coordinates.Coordinates(existLat, existLong),
                    UnitOfLength.Kilometers
                );
            return distance;
        }

        private static string StringFormater(string text)
        {
            var result = Regex.Replace(text, @"[^0-9a-zA-Z:,]+", " ");
            return result;
        }
    }
}