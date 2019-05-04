using Store.Data.Infrastructure;
using Store.Data.IRepositories;
using Store.Model.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Data.Repositories
{
    public class TagRepository : RepositoryBase<ItemTag>, ITagRepository
    {
        public TagRepository(IDbFactory dbFactory)
            : base(dbFactory) { }

        public ItemTag GetTagByName(string tagName)
        {
            return this.DbContext.ItemTags.Where(a => a.TagName.Equals(tagName.ToLower())).FirstOrDefault();
        }
    }
}
