using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Model.Models
{
    public enum Status
    {
        Awaiting,
        InProcess,
        Done
    }
    public enum PaymentMethod
    {
        PayPal,
        Econt
    }

    public class Order
    {
        public Order()
        {

        }
         
        public Order(Cart CartOfUser, UserDetails userDetails)
        {
            ProductsInCart = CartOfUser.ProductsInCart;

            FirstName = userDetails.FirstName;
            LastName = userDetails.LastName;
            Phone = userDetails.Phone;
            PaymentMethod = userDetails.PaymentMethod;
            Status = Status.Awaiting;
            OrderCreation = DateTime.Now;
            OrderCompletion = DateTime.Now;

            Country = userDetails.Country;
            Address = userDetails.Address;
            City = userDetails.City;
            Region = userDetails.Region;
            ZipCode = userDetails.ZipCode;
            this.ProductsInCart = CartOfUser.ProductsInCart;
            UserId = userDetails.UserId;

            Agreement = userDetails.Agreement;

            CalculateWholePrice();
        }

        private void CalculateWholePrice()
        {
            decimal wholePrice = 0;
            foreach (var item in ProductsInCart)
            {
                wholePrice += item.SubtotalForProduct;
            }
            WholePrice = wholePrice;
        }

        public int Id { get; set; }
        public List<ProductInCart> ProductsInCart { get; set; }
        [Required]
        public bool Agreement { get; set; }
        public Status Status { get; set; }
        public DateTime OrderCreation { get; set; }
        public DateTime OrderCompletion { get; set; }
        [Required]
        public String FirstName { get; set; }
        [Required]
        public String LastName { get; set; }


        [Required]
        public String Address { get; set; }
        [Required]
        public String City { get; set; }
        [Required]
        public String Country { get; set; }
        [Required]
        public String Phone { get; set; }
        [Required]
        public String Region { get; set; }
        [Required]
        public String ZipCode { get; set; }
        [Required]
        public PaymentMethod PaymentMethod { get; set; }

        public String PromoCode { get; set; }

        public int DiscountPercent { get; set; }

        public decimal WholePrice { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public void FinishOrder()
        {
            OrderCompletion = DateTime.Now;
            this.Status = Status.Done;
        }
    }
}
