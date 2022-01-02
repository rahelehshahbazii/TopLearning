using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TopLearn.Core.DTOs;
using TopLearn.Core.Services.Interfaces;

namespace TopLearn.Web.Areas.UserPanel.Controllers
{
    [Area("UserPanel")]
    [Authorize]
    public class HomeController : Controller
    {
        private IUserService _userService;

        public HomeController(IUserService userService)
        {
            _userService = userService;
        }
        public IActionResult Index()
        {
            return View(_userService.GetUserInformation(User.Identity.Name));
        }
        [Route("UserPanel/EditProfile")]
        public IActionResult EditProfile()
        {
            return View(_userService.GetDataForEditProfileUser(User.Identity.Name));
        }
        [Route("UserPanel/EditProfile")]
        [HttpPost]
        public IActionResult EditProfile(EditProfileViewModel Profile)
        {
            if (!ModelState.IsValid)
            {
                return View(Profile);
            }
            _userService.EditProfile(User.Identity.Name, Profile);

            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/Login?EditProfile=true");
        }
        [Route("UserPanel/ChangePassword")]
        public IActionResult ChangePassword()
            {
            return View();
            }
        [Route("UserPanel/ChangePassword")]
        [HttpPost]
        public IActionResult ChangePassword(ChangePasswordViewModel Change)
        {
            string currentUserName = User.Identity.Name;
            if (!ModelState.IsValid)
                return View(Change);
            
            if(_userService.CompareOldPassword(Change.OldPassword, currentUserName))
            {
                ModelState.AddModelError("OldPassword", "کلمه عبور فعلی صحیح نمی باشد");
                return View(Change);
            }
            _userService.ChangeUserPassword(currentUserName,Change.Password);
            ViewBag.Issusscce = true;
            return View();
        }
   

    }
}
