using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Store.Model.Models
{
    public class UserDetails
    {
        public UserDetails()
        {

        }

        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Region { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string ZipCode { get; set; }
        public string PromoCode { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public PaymentMethod PaymentMethod { get; set; }
        [Required]
        public bool Agreement { get; set; }
        public string UserId { get; set; }

    }
}