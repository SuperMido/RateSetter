using System;
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

            if (DoesUserExist(model.User, model.Address))
            {
                return false;
            }
            if (!DoesReferralCodeValid(model.User.ReferralCode))
            {
                return false;
            }

            return true;
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
        
        private static string ReversePartOfString(string text, int start, int length)
        {
            var chars = text.ToCharArray();

            for (int a = start, b = start + length - 1; a < b; ++a, --b)
            {
                (chars[a], chars[b]) = (chars[b], chars[a]);
            }

            return new string(chars);
        }

        private bool DoesReferralCodeValid(string code)
        {
            var stringArray = Array.Empty<string>();
            var listReferCode = _context.Users.Select(u => u.ReferralCode).ToList();
            if (code.Length % 2 == 0)
            {
                for (var i = 0; i <= code.Length / 2; i++)
                {
                    var tempString = ReversePartOfString(code, i, 3);
                    if (listReferCode.Contains(tempString))
                    {
                        return false;
                    }
                }
            }
            else
            {
                for (var i = 0; i <= code.Length / 2 + 1; i++)
                {
                    var tempString = ReversePartOfString(code, i, 3);
                    if (listReferCode.Contains(tempString))
                    {
                        return false;
                    }
                }
            }

            return true;
        }
        
    }
}