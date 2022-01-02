using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TopLearn.Core.Convertors;
using TopLearn.Core.DTOs;
using TopLearn.Core.Generator;
using TopLearn.Core.Security;
using TopLearn.Core.Senders;
using TopLearn.Core.Services.Interfaces;
using TopLearn.DataLayer.Entities.User;

namespace TopLearn.Web.Controllers
{
    public class AccountController : Controller
    {
        private IUserService _userService;
        private IViewRenderService _viewRender;
        public AccountController(IUserService userService, IViewRenderService viewRender)
        {
            _userService = userService;
            _viewRender = viewRender;
        }
        #region Register
        [Route("Register")]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [Route("Register")]
        public IActionResult Register(RegisterViewModel Register)
        {
           if(!ModelState.IsValid)
            {
                return View(Register);
            }
            if(_userService.IsExistUserName(Register.UserName))
            {
                ModelState.AddModelError("UserName", "نام کاربری معتبر نمی باشد");
                return View(Register);
            }

            if (_userService.IsExistEmail(FixedText.FixEmail(Register.Email)))
            {
                ModelState.AddModelError("UserName", "ایمیل معتبر نمی باشد");
                return View(Register);
            }

            User user = new User()
            {
                ActiveCode = NameGenerator.GenerateUniqCode(),
                Email = FixedText.FixEmail(Register.Email),
                IsActive = false,
                Password = PasswordHelper.EncodePasswordMd5(Register.Password),
                RegisterDate = DateTime.Now,
                UserAvatar= "Defult.jpg",
                UserName=Register.UserName
            };
            _userService.AddUser(user);
            #region Send Activation Email
            string body = _viewRender.RenderToStringAsync("_ActiveEmail", user);
            SendEmail.Send(user.Email,"فعالسازی",body);


            #endregion

            //TODO send Activation Email
            return View("SuccessRegister",user);
        }

        #endregion

        #region Login
        [Route("Login")]
        public ActionResult Login(bool EditPtrofile =false)
        {
            ViewBag.EditProfile = EditPtrofile;
            return View();
        }

        [HttpPost]
        [Route("Login")]
        public ActionResult Login(LoginViewModel login,string ReturnUrl="/")
        {
            if( !ModelState.IsValid )
            {
                return View(login);
            }

            var user = _userService.LoginUser(login);
            if (user!=null)
            {
                if (user.IsActive)
                {
                    var claims = new List<Claim>()
                    {
                       new Claim(ClaimTypes.NameIdentifier,user.UserId.ToString()),
                      new Claim(ClaimTypes.Name,user.UserName)
                    };
                    var identity = new ClaimsIdentity(claims,CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);
                    var properties = new AuthenticationProperties
                    {
                        IsPersistent = login.RememberMe
                    };
                    HttpContext.SignInAsync(principal, properties);
                    ViewBag.Issuccess = true;
                    if ( ReturnUrl !="/")
                    {
                        return Redirect(ReturnUrl);
                    }
                    return View();
                }
                else
                {
                    ModelState.AddModelError("Email", "حساب کاربری شما فعال نمی باشد");
                }
               
            }
            ModelState.AddModelError("Email", "کاربری با مشخصات داده شده یافت نشد");
            return View(login);
        }

        #endregion

        #region Active Acoount
        public IActionResult ActiveAccount(string id)
        {
            ViewBag.IsActive = _userService.ActiveAccount(id);
            {

            }
            return View();
        }

        #endregion

        #region Logout
        [Route("Logout")]
         public IActionResult Logout()
         {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/Login");
         }

        #endregion
        #region ForgotPassword
        [Route("ForgotPassword")]
        public ActionResult ForgotPassword()
        {

            return View();
        }
        [HttpPost]
        [Route("ForgotPassword")]
        public ActionResult ForgotPassword(ForgotPasswordViewModel forgot)
        {
            if (!ModelState.IsValid)
                return View(forgot);

            string FixedEmail = FixedText.FixEmail(forgot.Email);
            User user = _userService.GetUserByEmail(FixedEmail);
            if (user == null)
            {
                ModelState.AddModelError("Email", "کاربری یافت نشد");
                return View(forgot);
            }

            string Bodyemail = _viewRender.RenderToStringAsync("ForgotPassword", user);
            SendEmail.Send(user.Email,"بازیابی حساب کاربری",Bodyemail);
            ViewBag.Issuccess = true;

            return View();
        }

        #endregion

        #region Reset Password

        public ActionResult ResetPassword(string Id)
        {
            return View(new ResetPasswordViewModel()
            {
                ActiveCode = Id

            }); 

        }

        [HttpPost]
        public ActionResult ResetPassword(ResetPasswordViewModel reset)
        {
            if (!ModelState.IsValid)
                return View(reset);

            var user = _userService.GetUserByActiveCode(reset.ActiveCode);
            if (user == null)
                return NotFound();
            string HashNewPassword = PasswordHelper.EncodePasswordMd5(reset.Password);
            user.Password = HashNewPassword;
            _userService.UpdateUser(user);
            return Redirect("/Login");

        }
        #endregion
    }
}
