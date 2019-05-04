using Microsoft.AspNet.Identity;
using Store.Model.Models;
using Store.Service;
using Store.Service.IServices;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Store.Ta5FabrixsMVC.Controllers
{
    public class CatalogueController : Controller
    {
        public IProductCategoryService CategoryService { get; set; }
        public IProductService ProductService { get; set; }
        public IPromoService PromosService { get; set; }
        public IOrderService OrderService { get; set; }
        public IApplicationUserService UserService { get; set; }
        public IProductInCartService ProductsInCartService { get; set; }
        public IUserDetailsService UserDetailsService { get; set; }
        public IPromoCodeService PromoCodeService { get; set; }
        public ICookieCartService CookieCartService { get; set; }
        public ICartService CartService { get; set; }

        public CatalogueController(ICartService cartService, IPromoCodeService promoCodeService, ICookieCartService cookieCartService, IUserDetailsService userDetailsService, IOrderService orderService, IApplicationUserService userService, IProductInCartService productsInCartService, IProductCategoryService categoryService, IProductService productService, IPromoService promosService)
        {
            this.CookieCartService = cookieCartService;
            this.CartService = cartService;
            this.PromoCodeService = promoCodeService;
            this.CategoryService = categoryService;
            this.UserDetailsService = userDetailsService;
            this.OrderService = orderService;
            this.ProductService = productService;
            this.PromosService = promosService;
            this.UserService = userService;
            this.ProductsInCartService = productsInCartService;
        }
        
        public ActionResult Index(string Collection = "", string sortingType = "")
        {
            CategorySortingType cst = new CategorySortingType();
            ProductCategory categoryModel = new ProductCategory();
            if (!Collection.Equals("") && Collection != null)
            {
                categoryModel = CategoryService.GetCategoryAndItsProducts(Collection);
                categoryModel.Products = categoryModel
                    .Products
                    .GroupBy(a => a.Name)
                    .Select(x => x.First())
                    .OrderByDescending(a => a.Quantity)
                    .ThenByDescending(a => a.Id)
                    .ToList();
                if (sortingType.Equals("A-Z"))
                {
                    categoryModel.Products = categoryModel.Products.OrderBy(a => a.Name).ToList();
                }
                else if (sortingType.Equals("Low-High Price"))
                {
                    categoryModel.Products = categoryModel.Products.OrderBy(a => a.PriceEU).ToList();
                }
                else if (sortingType.Equals("High-Low Price"))
                {
                    categoryModel.Products = categoryModel.Products.OrderByDescending(a => a.PriceEU).ToList();
                }
                else
                {

                }
                cst.productCategory = categoryModel;
                cst.sortingType = sortingType;
                return View(cst);
            }
            categoryModel = CategoryService.GetCategoriesAndProducts()[0];
            cst.productCategory = categoryModel;
            cst.sortingType = sortingType;
            return View(cst);
        }

        [HttpGet]
        public ActionResult Checkout()
        {            
            if (User.Identity.IsAuthenticated && UserService.GetApplicationUser(User.Identity.GetUserId()).Cart.ProductsInCart.Count>0)
                return View();
            else if (!User.Identity.IsAuthenticated && Request.Cookies["Guest"] != null)
            {
                if (CookieCartService.GetCookieCart(Request.Cookies["Guest"].Value).ProductsInCart.Count > 0)
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("Cart", "Manage");
                }
            }
            else
            {
                return RedirectToAction("Cart","Manage");
            }
        }

        public void CreateAndSaveOrder(Order newOrder)
        {
            foreach (var item in newOrder.ProductsInCart)
            {
                item.SinglePrice = item.Product.PriceEU;
            }
            OrderService.CreateOrder(newOrder);
            
            if (User.Identity.IsAuthenticated)
            {
                ApplicationUser user = UserService.GetApplicationUser(User.Identity.GetUserId());
                user.Cart.ProductsInCart.Clear();
                UserService.UpdateApplicationUser(user);
            }
            else
            {
                if (Request.Cookies["Guest"] != null)
                {
                    foreach (var item in newOrder.ProductsInCart)
                    {
                        item.CookieCartId = null;
                    }
                }
            }

            OrderService.SaveOrder();
        }

        public ActionResult ConfirmOrder(Order order)
        {
            return View(order);
        }

        public ActionResult FinishOrder(UserDetails submitedDetails = null)
        {
            Order order = new Order();
            Cart cartOfUser = new Cart();
            if (User.Identity.IsAuthenticated)
                cartOfUser = CartService.GetCart(User.Identity.GetUserId());
            else
            {
                if (Request.Cookies["Guest"] != null)
                {
                    cartOfUser = CartService.GetCart(Request.Cookies["Guest"].Value);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            UserDetails details = new UserDetails();

            if (User.Identity.IsAuthenticated)
            {
                details = UserDetailsService.GetUserDetails(User.Identity.GetUserId());
            }
            else
            {
                details = UserDetailsService.GetUserDetails(Request.Cookies["Guest"].Value);
                details.UserId = "guestid";
            }
                
            if (cartOfUser.ProductsInCart.Count > 0)
            {
                Order newOrder = new Order(cartOfUser, details);
                
                if (!details.PromoCode.Equals(""))
                {
                    newOrder = ApplyPromoCode(newOrder, details.PromoCode);
                }
                
                try
                {
                    CreateAndSaveOrder(newOrder);
                    return RedirectToAction("Index", "Home", new { message = "OrderPlaced" });
                }
                catch (DbEntityValidationException e)
                {
                    return RedirectToAction("Index", "Home");
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
                return RedirectToAction("Index", "Manage");

            }
            return RedirectToAction("Cart", "Manage");
        }
        private Order ApplyPromoCode(Order order, string PromoCode)
        {
            PromoCode code = PromoCodeService.GetPromoCodeByName(PromoCode);

            if (code != null)
            {
                order.PromoCode = code.Code;
                order.DiscountPercent = code.PercentageDiscount;
                order.WholePrice = order.WholePrice * (100 - code.PercentageDiscount) / 100;
            }

            return order;
        }
       
        public ActionResult NewReleases()
        {
            var newItems = ProductService.GetNewProducts();
            return View(newItems);
        }

        public ActionResult Search(string For = "")
        {
            var newItems = ProductService.GetProductsByName(For);
            return View(newItems);
        }

        public PartialViewResult ListSales()
        {
            var sales = PromosService.GetEndingPromos();
            return PartialView("~/Views/Shared/_ListSales.cshtml", sales);
        }

        public ActionResult EditQuantity(int quantity, int productId)
        {
            ProductInCart pic = ProductsInCartService.GetProductInCart(productId);
            try
            {
                pic.Quantity = quantity;
                pic.SubtotalForProduct = quantity * pic.Product.PriceEU;
                ProductsInCartService.UpdateProductInCart(pic);
                ProductsInCartService.SaveProductInCart();
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
            return Json("Success!",JsonRequestBehavior.AllowGet);
        }

        public ActionResult Sale()
        {
            var promotions = PromosService.GetEndingPromos();
            return View(promotions);
        }

        public ActionResult Item(int id)
        {
            var item = ProductService.GetProduct(id);
            return View(item);
        }

        [HttpGet]
        public PartialViewResult ListSizes(string Name)
        {
            var sizes = ProductService.GetProductSizes(Name);
            return PartialView("~/Views/Shared/_ListSizes.cshtml", sizes);
        }

        [HttpGet]
        public PartialViewResult RelatedItems(int Id)
        {
            List<Product> relatedItems = ProductService.GetRelatedItems(Id);
            return PartialView("~/Views/Shared/_RelatedItems.cshtml", relatedItems);
        }

        [HttpPost]
        public ActionResult AddNewspaperMember(string mailName)
        {
            this.UserService.AddMail(mailName);
            this.UserService.SaveApplicationUser();
            return Json("Thanks for subscribing!", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ListSizes(int ProdId, int Quantity)
        {
            ProductInCart productInCart = new ProductInCart();
            Product actualProduct = ProductService.GetProduct(ProdId);

            productInCart.ProductId = ProdId;
            productInCart.Quantity = Quantity;
            productInCart.SubtotalForProduct = Decimal.Multiply(productInCart.Quantity,actualProduct.PriceEU);
            

            if (User.Identity.IsAuthenticated)
            {
                var UserId = User.Identity.GetUserId();
                var user = UserService.GetApplicationUser(UserId);
                var product = user.Cart.ProductsInCart
                    .Where(a => a.CartId == UserId && a.ProductId == productInCart.ProductId && a.OrderId == null)
                    .FirstOrDefault();
                if (product != null)
                {
                    product.Quantity += productInCart.Quantity;
                    product.SubtotalForProduct += productInCart.SubtotalForProduct;
                }
                else
                {
                    user.Cart.ProductsInCart.Add(productInCart);
                }
                UserService.UpdateApplicationUser(user);
                UserService.SaveApplicationUser();
            }
            else
            {
                Store.Model.Models.Cryptor cryptor = new Cryptor();
                if(Request.Cookies["Guest"] == null){
                    HttpCookie GuestCookie = new HttpCookie("Guest");
                    Random rnd = new Random();
                    int randomNumber1 = rnd.Next(1, 10);
                    int randomNumber2 = rnd.Next(11, 200);
                    string GuestValue = cryptor.Encrypt(DateTime.Now.ToString() + randomNumber1 + randomNumber2, true);
                    GuestCookie.Value = GuestValue;
                    GuestCookie.Expires = DateTime.Now.AddHours(1);
                    Response.Cookies.Add(GuestCookie);
                    try
                    {
                        CookieCart cookieCart = new CookieCart();
                        cookieCart.CookieId = GuestValue;
                        cookieCart.ProductsInCart = new List<ProductInCart>();
                        cookieCart.WholePrice = 0;
                        CookieCartService.CreateCookieCart(cookieCart);
                        CookieCartService.SaveCookieCart();


                        var product = cookieCart.ProductsInCart
                        .Where(a => a.ProductId == productInCart.ProductId && a.OrderId == null)
                        .FirstOrDefault();
                        if (product != null)
                        {
                            product.Quantity += productInCart.Quantity;
                            product.SubtotalForProduct += productInCart.SubtotalForProduct;
                        }
                        else
                        {
                            cookieCart.ProductsInCart.Add(productInCart);
                        }
                        CookieCartService.UpdateCookieCart(cookieCart);
                        CookieCartService.SaveCookieCart();
                    }
                    catch (Exception E)
                    {
                        throw new Exception(E.ToString());
                    }

                }
                else
                {
                    try
                    {
                        CookieCart cookieCart = CookieCartService.GetCookieCart(Request.Cookies["Guest"].Value);

                        var product = cookieCart.ProductsInCart
                            .Where(a => a.ProductId == productInCart.ProductId && a.OrderId == null)
                            .FirstOrDefault();
                        if (product != null)
                        {
                            product.Quantity += productInCart.Quantity;
                            product.SubtotalForProduct += productInCart.SubtotalForProduct;
                        }
                        else
                        {
                            cookieCart.ProductsInCart.Add(productInCart);
                        }
                        CookieCartService.UpdateCookieCart(cookieCart);
                        CookieCartService.SaveCookieCart();
                    }
                    catch (Exception E)
                    {
                        throw new Exception(E.ToString());
                    }
                }
                return RedirectToAction("Cart", "Manage");
            }
            return RedirectToAction("Cart", "Manage");
        }
    }
}