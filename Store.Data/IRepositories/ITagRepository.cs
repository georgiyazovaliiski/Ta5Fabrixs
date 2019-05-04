﻿using Store.Data.Infrastructure;
using Store.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Data.IRepositories
{
    public interface ITagRepository : IRepository<ItemTag>
    {
        ItemTag GetTagByName(string tagName);
    }
}
