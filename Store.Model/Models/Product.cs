using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Store.Model.Models
{
    public enum Size
    {
        NoSize,
        XS,
        S,
        M,
        L,
        XL,
        XXL
    }
    public class Product
    {
        public Product()
        {

        }

        public Product(String Name, String Description, int Quantity, decimal PriceEU, Size Size, string CategoryName)
        {
            this.Name = Name;
            this.Description = Description;
            this.Quantity = Quantity;
            this.PriceEU = PriceEU;
            this.OriginalPriceEU = PriceEU;
            this.PriceLV = Decimal.Multiply(this.PriceEU, 2);
            this.Size = Size;
            this.Tags = new List<ItemTag>();         //FIX THIS LATER
            this.Images = new List<Image>();
            this.CategoryId = 1;
        }

        public int Id { get; set; }
        [Required]
        public String Name { get; set; }
        [Required]
        public String Description { get; set; }
        [Required]
        [Range(0,300)]
        public int Quantity { get; set; }
        [Required]
        [Range(0.5, 1000)]
        public decimal PriceEU { get; set; }

        public decimal OriginalPriceEU { get; set; }
        public decimal PriceLV { get; set; }
        [Required]
        public Size Size { get; set; }

        public DateTime ReleaseDate { get; set; }

        [NotMapped]
        [Required]
        [DefaultValue("Ta5Fabrixs")]
        public string TagsText { get; set; }
        
        [NotMapped]
        [Display(Name = "Browse File")]
        public HttpPostedFileBase[] Files { get; set; }

        public List<ItemTag> Tags { get; set; }
        
        private List<Image> _Images { get; set; }
        public List<Image> Images
        {
            get
            {
                return _Images;
            }
            set
            {
                _Images = value;
            }
        }


        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public virtual ProductCategory Category { get; set; }

        [ForeignKey("Promo")]
        public int PromoId { get; set; }
        public virtual Promo Promo { get; set; }
    }
}
