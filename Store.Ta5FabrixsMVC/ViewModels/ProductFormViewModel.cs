using Store.Model.Models;
using Store.Ta5FabrixsMVC.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Store.Ta5FabrixsMVC
{
    public class ProductFormViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public decimal PriceEU { get; set; }
        public Size Size { get; set; }
        public HttpPostedFileBase[] Files { get; set; }
        public List<ItemTagViewModel> Tags { get; set; }
        public string TagsText { get; set; }
        public int CategoryId { get; set; }
    }
}