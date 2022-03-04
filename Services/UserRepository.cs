using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        
        [TempData]
        public string StatusMessage { get; set; }

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool IsMatch(UserAddressViewModel model)
        {
            if (!IsValidDistance(model.Address))
            {
                return false;
            }

            return !DoesUserExist(model.User, model.Address);
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
            if (IsMatch(model))
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
            var userAddressesList = _context.Addresses.ToList();
            return !userAddressesList.Any() || 
                   userAddressesList.Where(a => !(GetDistance(newUser, a) <= 0.5)).ToList().Any();
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

        private bool DoesUserExist(User user, Address address)
        {
            var userInDb = _context.Users.Include(a => a.Address).Where(u =>
                u.Name == user.Name && u.Address.StreetAddress == address.StreetAddress).ToList();

            return userInDb.Any();
        }
    }
}