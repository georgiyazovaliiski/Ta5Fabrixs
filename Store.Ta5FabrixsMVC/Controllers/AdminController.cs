using AutoMapper;
using Store.Model.Models;
using Store.Service.IServices;
using Store.Ta5FabrixsMVC.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Store.Ta5FabrixsMVC.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        public IProductService ProductService { get; set; }
        public IProductCategoryService CategoryService { get; set; }
        public ITagService TagService { get; set; }
        public IImageService ImageService { get; set; }
        public ILayoutService LayoutService { get; set; }
        public IPromoService PromoService { get; set; }
        public IOrderService OrderService { get; set; }
        public AdminController(
            IProductService productService, 
            IProductCategoryService productCategoryService,
            ITagService tagService,
            IImageService imageService,
            ILayoutService layoutService,
            IPromoService promoService,
            IOrderService orderService
            )
        {
            CategoryService = productCategoryService;
            ProductService = productService;
            TagService = tagService;
            ImageService = imageService;
            LayoutService = layoutService;
            PromoService = promoService;
            OrderService = orderService;
        }

        public ActionResult RestrictInformation()
        {
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Orders()
        {
            return View();
        }

        public PartialViewResult ListAllOrders()
        {
            List<Order> Orders = OrderService.GetAll().OrderBy(a=>a.Status).ThenByDescending(a=>a.OrderCreation).ToList();
            return PartialView("~/Views/Shared/_ListAllOrders.cshtml", Orders);
        }

        public ActionResult FinishOrder(int id)
        {
            Order order = OrderService.GetOrder(id);
            try
            {
                foreach (var prod in order.ProductsInCart)
                {
                    Product product = ProductService.GetProduct(prod.Product.Id);
                    product.TagsText = "-";
                    product.Quantity -= prod.Quantity;
                    if (product.Quantity < 0)
                    {
                        product.Quantity = 0;
                    }
                    ProductService.UpdateProduct(product);
                    ProductService.SaveProduct();
                }

                order.FinishOrder();
                OrderService.UpdateOrder(order);
                OrderService.SaveOrder();
            }
            catch (DbEntityValidationException e)
            {
                var errorfeed = "";
                foreach (var eve in e.EntityValidationErrors)
                {
                    errorfeed += String.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        errorfeed += String.Format("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw new Exception(errorfeed);
            }
            return RedirectToAction("Orders", id);
        }

        public ActionResult RemoveOrder(int id)
        {
            var removing = OrderService.GetOrder(id);
            OrderService.RemoveOrder(removing);
            OrderService.SaveOrder();
            return RedirectToAction("Orders");
        }

        public PartialViewResult OrdersSection(int? id)
        {
            if (id == null)
            {
                return PartialView("~/Views/Shared/_OrdersSection.cshtml");
            }
            else
            {
                return PartialView("~/Views/Shared/_OrdersSection.cshtml", OrderService.GetOrder(id ?? 0));
            }
        }

        public ActionResult Index(string Category = "")
        {
            if (checkIfAdmin())
            {
                return RedirectToAction("RestrictInformation");
            }
            return RedirectToAction("Items", new { Category = Category });
        }

        public ActionResult Categories()
        {
            if (checkIfAdmin())
            {
                return RedirectToAction("RestrictInformation");
            }
            return View();
        }

        public PartialViewResult ListCategories()
        {
            return PartialView("~/Views/Shared/_ListCategories.cshtml", CategoryService.GetProductCategories());
        }
        
        public ActionResult Items(String Category = "", String errorMessage = "NoErrors")
        {
            if (checkIfAdmin())
            {
                return RedirectToAction("RestrictInformation");
            }
            var category = CategoryService.GetProductCategory(Category);
            category.UrlImage = errorMessage;
            return View(category);
        }

        public ActionResult ListItems(String Category = "")
        {
            List<Product> items = ProductService.GetProducts(Category).OrderBy(a=>a.Name).ThenBy(a=>a.Id).ToList();
            return PartialView("~/Views/Shared/_ListItems.cshtml", items);
        }

        public ActionResult AdminOptionsMenu()
        {
            var categories = CategoryService.GetProductCategories();
            return PartialView("~/Views/Shared/_AdminOptionsMenu.cshtml", categories);
        }

        [ChildActionOnly]
        [HttpGet]
        public ActionResult CreateItem(String Category = "")
        {
            var productModel = new Product();
            var category = CategoryService.GetProductCategory(Category);
            productModel.CategoryId = category.Id;
            productModel.Category = category;
            return PartialView("~/Views/Shared/_CreateItem.cshtml", productModel);
        }

        [HttpPost]
        public ActionResult CreateItem(Product product)
        {
            if (ModelState.IsValid)
            {

                product.ReleaseDate = DateTime.Now;
                product.PriceLV = product.PriceEU * 2;
                product.OriginalPriceEU = product.PriceEU;
                product.Tags = new List<ItemTag>();
                product.Images = new List<Image>();
                var productTags = product.TagsText
                                        .Trim()
                                        .Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries)
                                        .ToList();
                foreach (var p in productTags)
                {
                    var checkIfTagExists = TagService.CheckIfTagExists(p);
                    if (checkIfTagExists != null)
                    {
                        product.Tags.Add(checkIfTagExists);
                    }
                    else
                    {
                        var itemTag = new ItemTag();
                        itemTag.TagName = p;
                        product.Tags.Add(itemTag);
                    }
                }
                foreach (HttpPostedFileBase file in product.Files)
                {
                    if (file != null)
                    {
                        var InputFileName = Path.GetFileName(file.FileName);
                        var timeNow = DateTime.Now;
                        InputFileName = timeNow.Year + "-" +
                                        timeNow.Month + "-" +
                                        timeNow.Day + "-" +
                                        timeNow.Hour + "-" +
                                        timeNow.Minute + "-" +
                                        timeNow.Second + "-" +
                                        timeNow.Millisecond + "-" +
                                        InputFileName;
                        var ServerSavePath = Path.Combine(Server.MapPath("~/Content/Images/ProductImages/") + InputFileName);
                        product.Images.Add(new Image(InputFileName));

                        file.SaveAs(ServerSavePath);

                        ViewBag.UploadStatus = product.Files.Count().ToString() + " files uploaded successfully.";
                    }

                }

                ProductService.CreateProduct(product);
                var category = CategoryService.GetProductCategory(product.CategoryId);
                ProductService.SaveProduct();
                return RedirectToAction("Items", new { Category = category.Name });
            }
            else
            {
                var category = CategoryService.GetProductCategory(product.CategoryId);
                var errorMessage2 = "";
                var errors = ModelState.Select(x => x.Value.Errors)
                       .Where(y => y.Count > 0)
                       .ToList();
                foreach (var error in errors)
                {
                    foreach (var e in error)
                    {
                        errorMessage2 += e.ErrorMessage;
                    }
                }
                return RedirectToAction("Items", "Admin", new { Category = category.Name, errorMessage = errorMessage2 });
            }
        }

        public ActionResult GetItem(int id)
        {
            var product = ProductService.GetProduct(id);
            product.TagsText = "";
            foreach (var tag in product.Tags)
            {
                product.TagsText += tag.TagName + ",";
            }
            product.TagsText = product.TagsText.Trim().Substring(0, product.TagsText.Length - 1);
            
            var viewModelProduct = Mapper.Map<Product, ProductViewModel>(product);
            return Json(viewModelProduct, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCategory(int id)
        {
            var category = CategoryService.GetProductCategory(id);
            return Json(category, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public PartialViewResult EditCategory()
        {
            return PartialView("~/Views/Shared/_EditCategory.cshtml");
        }

        [HttpPost]
        public ActionResult EditCategory(ProductCategory updatingCategory)
        {
            var realCategory = CategoryService.GetProductCategory(updatingCategory.Id);

            realCategory.Name = updatingCategory.Name;
            
            try
            {
                if (updatingCategory.File != null)
                {
                    var InputFileName = Path.GetFileName(updatingCategory.File.FileName);
                    var timeNow = DateTime.Now;
                    InputFileName = "Category" + updatingCategory.Id + ".jpg";
                    var ServerSavePath = Path.Combine(Server.MapPath("~/Content/Images/CategoryImages/") + InputFileName);
                    realCategory.UrlImage = InputFileName;
          
                    updatingCategory.File.SaveAs(ServerSavePath);
          
                    ViewBag.UploadStatus = "1 file uploaded successfully.";
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
            CategoryService.UpdateProductCategory(realCategory);

            CategoryService.SaveProductCategory();

            return RedirectToAction("Categories");
        }

        [HttpGet]
        public PartialViewResult EditItem()
        {
            return PartialView("~/Views/Shared/_EditItem.cshtml");
        }

        [HttpPost]
        public ActionResult EditItem(ProductViewModel realProduct)
        {
            var product = Mapper.Map<ProductViewModel, Product>(realProduct);
            var updatingProduct = ProductService.GetProduct(realProduct.Id);

            updatingProduct.TagsText = "tagstext";
            updatingProduct.Name = product.Name;
            updatingProduct.Description = product.Description;
            updatingProduct.CategoryId = product.CategoryId;
            if(updatingProduct.PriceEU == updatingProduct.OriginalPriceEU)
            {
                updatingProduct.PriceEU = product.PriceEU;
            }
            updatingProduct.OriginalPriceEU = product.PriceEU;
            updatingProduct.PriceLV = Decimal.Multiply(product.PriceEU,2);
            updatingProduct.Quantity = product.Quantity;
            updatingProduct.Size = product.Size;

            product.Tags = new List<ItemTag>();
            var productTags = product.TagsText
                                    .Trim()
                                    .Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries)
                                    .ToList();

            
            foreach (var p in productTags)
            {
                var checkIfTagExists = TagService.CheckIfTagExists(p);

                if (checkIfTagExists != null)
                {
                    product.Tags.Add(checkIfTagExists);
                }
                else
                {
                    var itemTag = new ItemTag();
                    itemTag.TagName = p;
                    product.Tags.Add(itemTag);
                }
            }
            updatingProduct.Tags = product.Tags;

            foreach (HttpPostedFileBase file in product.Files)
            {
                if (file != null)
                {
                    var InputFileName = Path.GetFileName(file.FileName);
                    var timeNow = DateTime.Now;
                    InputFileName = timeNow.Year + "-" +
                                    timeNow.Month + "-" +
                                    timeNow.Day + "-" +
                                    timeNow.Hour + "-" +
                                    timeNow.Minute + "-" +
                                    timeNow.Second + "-" +
                                    timeNow.Millisecond + "-" +
                                    InputFileName;
                    var ServerSavePath = Path.Combine(Server.MapPath("~/Content/Images/ProductImages/") + InputFileName);
                    updatingProduct.Images.Add(new Image(InputFileName));
            
                    file.SaveAs(ServerSavePath);
            
                    ViewBag.UploadStatus = product.Files.Count().ToString() + " files uploaded successfully.";
                }

            }

            try
            {
                ProductService.UpdateProduct(updatingProduct);
                ProductService.SaveProduct();
            }
            catch (DbEntityValidationException e)
            {
                string error = "";
                foreach (var eve in e.EntityValidationErrors)
                {
                    error += "Entity of type " + eve.Entry.Entity.GetType().Name + " in state " + eve.Entry.State + " has the following validation errors:\n";
                    foreach (var ve in eve.ValidationErrors)
                    {
                        error += "- Property: " + ve.PropertyName + ", Error: " + ve.ErrorMessage;
                    }
                }
                throw new Exception(error);
            }
 
            return RedirectToAction("Index", new { Category = updatingProduct.Category.Name });
        }

        public ActionResult Remove(int Id)
        {
            ProductService.RemoveProduct(Id);
            ProductService.SaveProduct();
            return RedirectToAction("Index");
        }

        public ActionResult LayoutCustomisation()
        {
            return View();
        }

        public PartialViewResult FrontCategoriesSelection(int selected1, int selected2, int selected3)
        {
            MainCategoryPicker mainCategoryPicker = new MainCategoryPicker();
            var categories = CategoryService.GetProductCategories();
            mainCategoryPicker.categories = categories;
            mainCategoryPicker.SelectedCategory1 = selected1;
            mainCategoryPicker.SelectedCategory2 = selected2;
            mainCategoryPicker.SelectedCategory3 = selected3;
            return PartialView("~/Views/Shared/_FrontCategoriesSelection.cshtml", mainCategoryPicker); 
        }

        [HttpGet]
        public PartialViewResult CustomisationSection()
        {
            var layoutLinks = LayoutService.GetLayout(1);
            return PartialView("~/Views/Shared/_CustomisationSection.cshtml", layoutLinks);
        }

        [HttpPost]
        public ActionResult EditLayout(LayoutModel layout)
        {
            layout.Id = 1;
            if (!layout.FrontCategoryBanner1String.Equals("0"))
                layout.FrontCategoryBanner1Id = int.Parse(layout.FrontCategoryBanner1String.Split('.').ToArray()[0]);
            if (!layout.FrontCategoryBanner2String.Equals("0"))
                layout.FrontCategoryBanner2Id = int.Parse(layout.FrontCategoryBanner2String.Split('.').ToArray()[0]);
            if (!layout.FrontCategoryBanner3String.Equals("0"))
                layout.FrontCategoryBanner3Id = int.Parse(layout.FrontCategoryBanner3String.Split('.').ToArray()[0]);

            if (layout.Banner1 != null)
            {
                var InputFileName = "Banner1.jpg";
                var ServerSavePath = Path.Combine(Server.MapPath("~/Content/Images/FrontImages/") + InputFileName);
                layout.Banner1Url = InputFileName;
    
                layout.Banner1.SaveAs(ServerSavePath);
    
                ViewBag.UploadStatus = "File uploaded successfully.";
            }
            if (layout.Banner2 != null)
            {
                var InputFileName = "Banner2.jpg";
                var ServerSavePath = Path.Combine(Server.MapPath("~/Content/Images/FrontImages/") + InputFileName);
                layout.Banner2Url = InputFileName;
    
                layout.Banner2.SaveAs(ServerSavePath);
            }
            if (layout.Banner3 != null)
            {
                var InputFileName = "Banner3.jpg";
                var ServerSavePath = Path.Combine(Server.MapPath("~/Content/Images/FrontImages/") + InputFileName);
                layout.Banner3Url = InputFileName;
       
                layout.Banner3.SaveAs(ServerSavePath);
            }
            if (layout.NewReleasesBanner != null)
            {
                var InputFileName = "NewReleases.jpg";
                var ServerSavePath = Path.Combine(Server.MapPath("~/Content/Images/FrontImages/") + InputFileName);
                layout.NewReleasesBannerUrl = InputFileName;

                layout.NewReleasesBanner.SaveAs(ServerSavePath);
            }


            LayoutService.UpdateLayout(layout);
            LayoutService.SaveLayout();
            return RedirectToAction("LayoutCustomisation");
        }

        public ActionResult RemoveImage(int Id)
        {
            string removingImageName = ImageService.RemoveImage(Id);
            deleteImage(removingImageName);
            ImageService.SaveImage();
            return Json("Successfully removed this image!",JsonRequestBehavior.AllowGet);
        }

        public ActionResult Promos()
        {
            return View();
        }

        [HttpGet]
        public PartialViewResult CreatePromo()
        {
            return PartialView("~/Views/Shared/_CreatePromo.cshtml");
        }

        [HttpPost]
        public ActionResult CreatePromo(Promo promo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (promo.EndDate < promo.StartDate)
                    {
                        return RedirectToAction("Promos");
                    }
                    if (promo.StartDate < DateTime.Now)
                    {
                        promo.StartDate = DateTime.Now;
                    }
                    promo.PromoPriceLV = promo.PromoPriceEU * 2;
                    promo.Active = true;
                    PromoService.CreatePromo(promo);

                    Product updatingPriceProduct = ProductService.GetProduct(promo.ProductId);
                    updatingPriceProduct.TagsText = "Ta5Fabrixs";

                    updatingPriceProduct.OriginalPriceEU = updatingPriceProduct.PriceEU;
                    updatingPriceProduct.PriceEU = promo.PromoPriceEU;

                    ProductService.UpdateProduct(updatingPriceProduct);
                    ProductService.SaveProduct();
                }
                return RedirectToAction("Promos");
            }
            catch (DbEntityValidationException e)
            {
                string resultingError = "";
                foreach (var eve in e.EntityValidationErrors)
                {
                    foreach (var ve in eve.ValidationErrors)
                    {
                        resultingError += "- Property: " + ve.PropertyName + ", Error: " + ve.ErrorMessage + "";
                    }
                }
                throw new Exception(resultingError);
            }
        }

        public ActionResult ch4sls()
        {
            CheckForSalesAndDoThem();
            return Json("Done.",JsonRequestBehavior.DenyGet);
        }

        public void CheckForSalesAndDoThem()
        {
            try
            {
                PromoService.UpdateAllPromos();
                PromoService.SavePromo();
            }
            catch (DbEntityValidationException e)
            {
                string resultingError = "";
                foreach (var eve in e.EntityValidationErrors)
                {
                    foreach (var ve in eve.ValidationErrors)
                    {
                        resultingError += "- Property: " + ve.PropertyName + ", Error: " + ve.ErrorMessage + "";
                    }
                }
                throw new Exception(resultingError);
            }
        }

        public ActionResult ListPromos()
        {
            var promos = PromoService.GetPromos();
            return PartialView("~/Views/Shared/_ListPromos.cshtml", promos);
        }

        public ActionResult RemovePromo(int PromoId)
        {
            try { 
            Promo promo = PromoService.GetPromo(PromoId);
            PromoService.SavePromo();

            Product product = ProductService.GetProduct(promo.ProductId);
                product.TagsText = "Ta5Fabrixs";
            product.PriceEU = product.OriginalPriceEU;

            PromoService.RemovePromo(promo.ProductId);
            PromoService.SavePromo();
            return RedirectToAction("Promos");
            } catch (DbEntityValidationException e)
            {
                string resultingError = "";
                foreach (var eve in e.EntityValidationErrors)
                {
                    foreach (var ve in eve.ValidationErrors)
                    {
                        resultingError += "- Property: " + ve.PropertyName + ", Error: " + ve.ErrorMessage + "";
                    }
            }
                throw new Exception(resultingError);
            }
        }

        public PartialViewResult SelectProductForPromo()
        {
            var categoriesAndProducts = CategoryService.GetCategoriesAndProducts();
            return PartialView("~/Views/Shared/_SelectProductForPromo.cshtml", categoriesAndProducts);
        }

        private void deleteImage(string name)
        {
            string path = System.Web.HttpContext.Current.Server.MapPath("~/Content/Images/ProductImages/" + name);
            FileInfo file = new FileInfo(path);
            if (file.Exists)
            {
                file.Delete();
            }
            else
            {
                throw new Exception("An error came along! Try again!");
            }
        }

        private bool checkIfAdmin()
        {
            if(User.Identity.IsAuthenticated && User.IsInRole("admin"))
                return false;
            else
            {
                return true;
            }
        }
    }
}