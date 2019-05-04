using Store.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.IServices
{
    // operations you want to expose
    public interface ITagService
    {
        ItemTag GetTag(int id);
        ItemTag CheckIfTagExists(string tagName);
        void CreateTag(ItemTag Tag);
        void RemoveTag(int id);
        void SaveTag();
    }
}
