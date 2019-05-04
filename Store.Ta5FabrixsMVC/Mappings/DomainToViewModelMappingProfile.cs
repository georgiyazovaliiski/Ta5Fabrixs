using AutoMapper;
using Store.Model;
using Store.Model.Models;
using Store.Ta5FabrixsMVC;
using Store.Ta5FabrixsMVC.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Store.Ta5FabrixsMVC
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "DomainToViewModelMappings"; }
        }

        protected override void Configure()
        {
            Mapper.CreateMap<Category,CategoryViewModel>();
            Mapper.CreateMap<Gadget, GadgetViewModel>();
            Mapper.CreateMap<Product, ProductViewModel>();
            Mapper.CreateMap<ItemTag, ItemTagViewModel>();
            Mapper.CreateMap<Image, ImageViewModel>();
            Mapper.CreateMap<LayoutModel, LayoutViewModel>();
            Mapper.CreateMap<ProductCategory, ProductCategoryViewModel>();
        }
    }
}