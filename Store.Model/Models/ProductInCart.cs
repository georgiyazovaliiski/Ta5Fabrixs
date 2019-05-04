using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Model.Models
{
    public class ProductInCart
    {
        public ProductInCart()
        {

        }

        public ProductInCart(Product product, int quantity, Cart cart)
        {
            ProductId = product.Id;
            Quantity = quantity;
            CartId = cart.UserId;
            calculateSubtotalPrice(product.PriceLV, quantity);
        }

        private void calculateSubtotalPrice(decimal priceLV, int quantity)
        {
            SubtotalForProduct = Decimal.Multiply(priceLV, quantity);
        }
    

        public int Id { get; set; }
        [ForeignKey("Product")]
        public int ProductId { get; set; }

        [ForeignKey("Order")]
        public int? OrderId { get; set; }

        [ForeignKey("CookieCart")]
        public string CookieCartId { get; set; }

        public int Quantity { get; set; }
        public decimal SubtotalForProduct { get; set; }

        public decimal SinglePrice { get; set; }
        

        [ForeignKey("Cart")]
        public string CartId { get; set; }

        public virtual Cart Cart { get; set; }

        public virtual CookieCart CookieCart { get; set; }

        public virtual Order Order { get; set; }

        public virtual Product Product { get; set; }
    }
}
