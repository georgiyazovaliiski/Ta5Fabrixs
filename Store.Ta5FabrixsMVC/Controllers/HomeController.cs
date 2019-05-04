using AutoMapper;
using Store.Model.Models;
using Store.Service.IServices;
using Store.Ta5FabrixsMVC.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Store.Ta5FabrixsMVC.Controllers
{
    //[RequireHttps]
    public class HomeController : Controller
    {
        public IProductCategoryService CategoryService;
        public ILayoutService LayoutService;
        public HomeController(IProductCategoryService categoryService, ILayoutService layoutService)
        {
            this.CategoryService = categoryService;
            this.LayoutService = layoutService;
        }
        public ActionResult Index(string message = "")
        {
            LayoutModel layout = LayoutService.GetLayout(1);
            layout.FrontCategory1 = CategoryService.GetProductCategory(layout.FrontCategoryBanner1Id);
            layout.FrontCategory2 = CategoryService.GetProductCategory(layout.FrontCategoryBanner2Id);
            layout.FrontCategory3 = CategoryService.GetProductCategory(layout.FrontCategoryBanner3Id);
            
            var layoutView = Mapper.Map<LayoutModel, LayoutViewModel>(layout);
            layoutView.Message = message;

            return View(layoutView);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public PartialViewResult MainMenu()
        {
            var categories = CategoryService.GetProductCategories();
            return PartialView("~/Views/Shared/_MainMenu.cshtml", categories);
        }
    }
}