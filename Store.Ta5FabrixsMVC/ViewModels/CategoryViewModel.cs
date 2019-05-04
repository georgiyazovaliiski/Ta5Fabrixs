using Store.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Store.Ta5FabrixsMVC
{
    public class CategoryViewModel
    {
        public int CategoryID { get; set; }
        public string Name { get; set; }

        public List<GadgetViewModel> Gadgets { get; set; }
    }
}