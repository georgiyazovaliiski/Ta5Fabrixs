using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Store.Ta5FabrixsMVC.ViewModels
{
    public class LayoutViewModel
    {
        public string Message { get; set; }

        public string Banner1Url { get; set; }
        public string Banner2Url { get; set; }
        public string Banner3Url { get; set; }
        public string NewReleasesBannerUrl { get; set; }

        public ProductCategoryViewModel FrontCategory1 { get; set; }
        public ProductCategoryViewModel FrontCategory2 { get; set; }
        public ProductCategoryViewModel FrontCategory3 { get; set; }
    }
}