using RateSetter.Models;

namespace RateSetter.ViewModels
{
    public class UserAddressViewModel
    {
        public Address Address { get; set; }
        public User User { get; set; }
        public string StatusMessage { get; set; }
    }
}