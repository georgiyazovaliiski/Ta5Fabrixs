using Store.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Store.Ta5FabrixsMVC.ViewModels
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public int Quantity { get; set; }
        public decimal PriceEU { get; set; }
        public decimal PriceLV { get; set; }
        public Size Size { get; set; }

        public List<ItemTagViewModel> Tags { get; set; }

        public List<ImageViewModel> Images { get; set; }
        public HttpPostedFileBase[] Files { get; set; }

        public String TagsText { get; set; }
        public int CategoryId { get; set; }
    }
}