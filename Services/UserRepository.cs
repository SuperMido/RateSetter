using System;
using System.Linq;
using RateSetter.Data;
using RateSetter.Models;
using RateSetter.Services.Coordinates;
using RateSetter.Services.Interfaces;

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
            var isValidDistance = IsValidDistance(newUser, existingUser);
            throw new System.NotImplementedException();
        }

        public bool CreateUser(User newUser)
        {
            throw new NotImplementedException();
        }

        private bool IsValidDistance(User newUser, User existingUser)
        {
            var existingUsers = _context.Users.Any(u => GetDistance(newUser.Address, u.Address) <= 500);
            return existingUsers;
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
    }
}