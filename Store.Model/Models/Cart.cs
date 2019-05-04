using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Store.Model.Models
{
    public class Cart
    {
        public Cart()
        {

        }
        
        public void CalculateWholePrice()
        {
            decimal wh = 0;
            foreach (var p in ProductsInCart)
            {
                wh += p.SubtotalForProduct;
            }
            this.WholePrice = wh;
        }

        [Key, ForeignKey("User")]
        public string UserId { get; set; }
        public List<ProductInCart> ProductsInCart { get; set; }
        public decimal WholePrice { get; set; }

        private DateTime _LastUpdated = DateTime.Now;
        public DateTime LastUpdated
        {
            get
            {
                return _LastUpdated;
            }
            set
            {
                _LastUpdated = value;
            }
        }

        public virtual ApplicationUser User { get; set; }
    }
}