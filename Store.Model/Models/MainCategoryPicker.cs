using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Model.Models
{
    public class MainCategoryPicker
    {
        public List<Store.Model.Models.ProductCategory> categories { get; set; }
        public int SelectedCategory1 { get; set; }
        public int SelectedCategory2 { get; set; }
        public int SelectedCategory3 { get; set; }
    }
}
