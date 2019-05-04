using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Store.Model.Models
{
    public class CookieCart
    {
        public CookieCart()
        {

        }
        [Key]
        public string CookieId { get; set; }
        public List<ProductInCart> ProductsInCart { get; set; }
        public decimal WholePrice { get; set; }
        public DateTime LastUpdated { get; set; }

        public void CalculateWholePrice()
        {
            decimal wh = 0;
            foreach (var p in this.ProductsInCart)
            {
                wh += p.SubtotalForProduct;
            }
            this.WholePrice = wh;
        }
        
    }
}