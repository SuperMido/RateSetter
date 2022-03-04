using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RateSetter.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
        public string ReferralCode { get; set; }
        [Display(Name = "Address")]
        public int AddressId { get; set; }
        [ForeignKey("AddressId")]
        public virtual Address Address { get; set; }
    }
}