using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Store.Model.Models
{
    public class LayoutModel
    {
        public LayoutModel()
        {

        }

        public int Id { get; set; }
        public string Banner1Url { get; set; }
        public string Banner2Url { get; set; }
        public string Banner3Url { get; set; }
        public string NewReleasesBannerUrl { get; set; }
        

        [NotMapped]
        public string FrontCategoryBanner1String { get; set; }
        [NotMapped]
        public string FrontCategoryBanner2String { get; set; }
        [NotMapped]
        public string FrontCategoryBanner3String { get; set; }
        
        [NotMapped]
        [Display(Name = "Choose Large Banner 1")]
        public HttpPostedFileBase Banner1 { get; set; }

        [NotMapped]
        [Display(Name = "Choose Large Banner 2")]
        public HttpPostedFileBase Banner2 { get; set; }

        [NotMapped]
        [Display(Name = "Choose Large Banner 3")]
        public HttpPostedFileBase Banner3 { get; set; }

        [NotMapped]
        [Display(Name = "Choose NewReleasesBanner")]
        public HttpPostedFileBase NewReleasesBanner { get; set; }
        
        public int FrontCategoryBanner1Id { get; set; }
        public int FrontCategoryBanner2Id { get; set; }
        public int FrontCategoryBanner3Id { get; set; }

        public virtual ProductCategory FrontCategory1 { get; set; }
        public virtual ProductCategory FrontCategory2 { get; set; }
        public virtual ProductCategory FrontCategory3 { get; set; }
    }
}
