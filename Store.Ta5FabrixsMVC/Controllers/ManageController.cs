using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Store.Model.Models;
using Store.Service.IServices;
using Store.Ta5FabrixsMVC.Models;

namespace Store.Ta5FabrixsMVC.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        public ICartService CartService;
        public IProductInCartService ProductsInCartService;
        public IUserDetailsService UserDetailsService;
        public IPromoCodeService PromoCodeService;
        public ICookieCartService CookieCartService;

        public ManageController(IPromoCodeService promoCodeService, ICookieCartService cookieCartService, ICartService cartService, IProductInCartService productInCartService, IUserDetailsService userDetailsService)
        {
            PromoCodeService = promoCodeService;
            CookieCartService = cookieCartService;
            CartService = cartService;
            ProductsInCartService = productInCartService;
            UserDetailsService = userDetailsService;
        }

        public ManageController(ApplicationUserManager userManager, ApplicationSignInManager signInManager, ICartService cartService)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            CartService = cartService;
        }

        public ActionResult RemoveItemFromCart(int id)
        {
            ProductsInCartService.RemoveProductInCart(id);
            ProductsInCartService.SaveProductInCart();
            return RedirectToAction("Cart");
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        
        [AllowAnonymous]
        public ActionResult Cart()
        {
            if (User.Identity.IsAuthenticated)
            {
                Cart cart = CartService.GetCart(User.Identity.GetUserId());
                cart.CalculateWholePrice();
                return View(cart);
            }
            else
            {
                Cart cart = CartService.GetCart(CheckForCookies());
                cart.CalculateWholePrice();
                return View(cart);
            }
        }

        public string CheckForCookies()
        {
            Cryptor cryptor = new Cryptor();
            if (Request.Cookies["Guest"] == null)
            {
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
                    return GuestValue;
                }
                catch (Exception E)
                {
                    throw new Exception(E.ToString());
                }
            }
            else
            {
                return Request.Cookies["Guest"].Value;
            }
        }

        public class ConfirmOrderProduct {
            public ConfirmOrderProduct()
            {

            }

            public ConfirmOrderProduct(int Id, string Name, decimal PriceEU, int Quantity)
            {
                this.Id = Id;
                this.Name = Name;
                this.PriceEU = PriceEU;
                this.Quantity = Quantity;
            }

            public string Name { get; set; }
            public int Id { get; set; }
            public decimal PriceEU { get; set; }
            public int Quantity { get; set; }
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult AddUserDetails(UserDetails UserDetails)
        {
            try {
                if (User.Identity.IsAuthenticated)
                {
                    UserDetails.UserId = User.Identity.GetUserId();
                }
                else
                {
                    UserDetails.UserId = Request.Cookies["Guest"].Value;
                }
                UserDetailsService.CreateUserDetails(UserDetails);
                UserDetailsService.SaveUserDetails();
            }
            catch (DbEntityValidationException e)
            {
                var errorfeed = UserDetails.FirstName + " ErrorFeed: ";
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
            return Json("Proceed With Confirming",JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public ActionResult GetCartItemsForCheckout(string PromoCode = "")
        {
            int DiscountPercentage = 0;
            if (PromoCodeService.GetPromoCodeByName(PromoCode) != null) { 
                DiscountPercentage = PromoCodeService.GetPromoCodeByName(PromoCode).PercentageDiscount;
            }
            List<ConfirmOrderProduct> FinalConfirmation = new List<ConfirmOrderProduct>();
            if (User.Identity.IsAuthenticated)
            {
                FinalConfirmation = CartService
                    .GetCart(User.Identity.GetUserId())
                    .ProductsInCart
                    .Select(a=>new ConfirmOrderProduct(a.Id, a.Product.Name, (a.Product.PriceEU * (100 - DiscountPercentage)) / 100, a.Quantity))
                    .ToList();
            }
            else
            {
                FinalConfirmation = CartService
                    .GetCart(Request.Cookies["Guest"].Value)
                    .ProductsInCart
                    .Select(a => new ConfirmOrderProduct(a.Id, a.Product.Name, (a.Product.PriceEU * (100 - DiscountPercentage)) / 100, a.Quantity))
                    .ToList();
            }
            return Json(FinalConfirmation, JsonRequestBehavior.AllowGet);
        }
        
        public async Task<ActionResult> Index(ManageMessageId? message)
        {
            if (User.IsInRole("admin"))
            {
                return RedirectToAction("Index", "Admin");
            }
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.SetTwoFactorSuccess ? "Your two-factor authentication provider has been set."
                : message == ManageMessageId.Error ? "An error has occurred."
                : message == ManageMessageId.AddPhoneSuccess ? "Your phone number was added."
                : message == ManageMessageId.RemovePhoneSuccess ? "Your phone number was removed."
                : "";

            var userId = User.Identity.GetUserId();
            var model = new IndexViewModel
            {
                HasPassword = HasPassword(),
                PhoneNumber = await UserManager.GetPhoneNumberAsync(userId),
                TwoFactor = await UserManager.GetTwoFactorEnabledAsync(userId),
                Logins = await UserManager.GetLoginsAsync(userId),
                BrowserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync(userId)
            };
            return View(model);
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            AddErrors(result);
            return View(model);
        }
        
        public ActionResult SetPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                if (result.Succeeded)
                {
                    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                    if (user != null)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    }
                    return RedirectToAction("Index", new { Message = ManageMessageId.SetPasswordSuccess });
                }
                AddErrors(result);
            }

            return View(model);
        }
        
        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

#region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        private bool HasPhoneNumber()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PhoneNumber != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            AddPhoneSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error
        }

#endregion
    }
}