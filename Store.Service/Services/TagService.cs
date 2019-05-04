using Store.Data.Infrastructure;
using Store.Data.IRepositories;
using Store.Data.Repositories;
using Store.Model;
using Store.Model.Models;
using Store.Service.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service
{
    public class TagService : ITagService
    {
        private readonly ITagRepository TagsRepository;
        private readonly IUnitOfWork unitOfWork;

        public TagService(ITagRepository TagsRepository, IUnitOfWork unitOfWork)
        {
            this.TagsRepository = TagsRepository;
            this.unitOfWork = unitOfWork;
        }

        #region ITagService Members

        public ItemTag GetTag(int id)
        {
            var Tag = TagsRepository.GetById(id);
            return Tag;
        }

        public ItemTag CheckIfTagExists(string tagName)
        {
            var Tag = TagsRepository.GetTagByName(tagName);
            if(Tag == null)
            {
                return null;
            }
            else
            {
                return Tag;
            }
        }

        public void CreateTag(ItemTag Tag)
        {
            TagsRepository.Add(Tag);
        }

        public void RemoveTag(int Id)
        {
            var Tag = TagsRepository.GetById(Id);
            TagsRepository.Delete(Tag);
        }

        public void SaveTag()
        {
            unitOfWork.Commit();
        }

        #endregion
    }
}
