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
    public class ViewModelToDomainMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "ViewModelToDomainMappings"; }
        }

        protected override void Configure()
        {
            Mapper.CreateMap<GadgetFormViewModel, Gadget>()
                .ForMember(g => g.Name, map => map.MapFrom(vm => vm.GadgetTitle))
                .ForMember(g => g.Description, map => map.MapFrom(vm => vm.GadgetDescription))
                .ForMember(g => g.Price, map => map.MapFrom(vm => vm.GadgetPrice))
                .ForMember(g => g.Image, map => map.MapFrom(vm => vm.File.FileName))
                .ForMember(g => g.CategoryID, map => map.MapFrom(vm => vm.GadgetCategory));

            Mapper.CreateMap<ProductViewModel, Product>()
                .ForMember(g => g.Id, map => map.MapFrom(vm => vm.Id))
                .ForMember(g => g.Name, map => map.MapFrom(vm => vm.Name))
                .ForMember(g => g.Description, map => map.MapFrom(vm => vm.Description))
                .ForMember(g => g.CategoryId, map => map.MapFrom(vm => vm.CategoryId))
                .ForMember(g => g.PriceEU, map => map.MapFrom(vm => vm.PriceEU))
                .ForMember(g => g.Quantity, map => map.MapFrom(vm => vm.Quantity))
                .ForMember(g => g.Size, map => map.MapFrom(vm => vm.Size))
                .ForMember(g => g.Tags, map => map.MapFrom(vm => vm.Tags))
                .ForMember(g => g.Files, map => map.MapFrom(vm => vm.Files))
                .ForMember(g => g.TagsText, map => map.MapFrom(vm => vm.TagsText));
        }
    }
}