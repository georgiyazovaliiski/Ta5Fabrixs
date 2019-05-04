using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace Store.Model.Models
{
    public class ProductCategory
    {
        public ProductCategory()
        {

        }
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        [NotMapped]
        [Display(Name = "Browse File")]
        public HttpPostedFileBase File { get; set; }

        public string UrlImage { get; set; }
        public List<Product> Products { get; set; }
    }
}