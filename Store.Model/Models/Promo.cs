using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Model.Models
{
    public class Promo
    {
        [Key, ForeignKey("Product")]
        public int ProductId { get; set; }

        [Required]
        public decimal PromoPriceEU { get; set; }
        public decimal PromoPriceLV { get; set; }
        
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool Active { get; set; }

        public virtual Product Product { get; set; }
    }
}
