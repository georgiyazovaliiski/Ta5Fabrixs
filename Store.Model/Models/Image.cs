using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Model.Models
{
    public class Image
    {
        public Image()
        {

        }
        public Image(string urlName)
        {
            this.UrlName = urlName;
        }
        public int Id { get; set; }
        public string UrlName { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }

        public virtual Product Product{ get; set; }
    }
}
