using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using TopLearn.Core.Services.Interfaces;
namespace TopLearn.Web.Controllers
{
    public class HomeController : Controller
    {
        private IUserService _userService;
        private ICourseService _courseService;

        public HomeController(IUserService userService, ICourseService courseService)
        {
            _userService = userService;
            _courseService = courseService;
        }
        public IActionResult Index()
        { 
           return View(_courseService.GetCourse().Item1);
        }
        [Route("OnlinePayment/{id}")]
        public IActionResult onlinePayment(int id)
        {
            if(HttpContext.Request.Query["Status"] !="" &&
              HttpContext.Request.Query["Status"].ToString().ToLower()=="ok" && 
              HttpContext.Request.Query["Authority"]!="" )
            {
                string authority = HttpContext.Request.Query["Authority"];
                var Wallet = _userService.GetWalletByWalletId(id);
                var payment = new ZarinpalSandbox.Payment(Wallet.Amount);
                var res = payment.Verification(authority).Result;
                if(res.Status==100)
                {
                    ViewBag.Code = res.RefId;
                    ViewBag.IsScuccess = true;
                    Wallet.IsPay = true;
                    _userService.UpdateWallet(Wallet);
                }
            }
            return View();
        }
        public IActionResult GetSubGroups(int id)
        {
            List<SelectListItem> list = new List<SelectListItem>()
            {
                new SelectListItem(){Text = "انتخاب کنید",Value = ""}
            };
            list.AddRange(_courseService.GetSubGroupForManageCourse(id));
            return Json(new SelectList(list, "Value", "Text"));
        }
        [HttpPost]
        [Route("file-upload")]
        public IActionResult UploadImage(IFormFile upload, string CKEditorFuncNum, string CKEditor, string langCode)
        {
            if (upload.Length <= 0) return null;
            var fileName = Guid.NewGuid() + Path.GetExtension(upload.FileName).ToLower();
            var path = Path.Combine(
                Directory.GetCurrentDirectory(), "wwwroot/MyImages",
                fileName);
            using (var stream = new FileStream(path, FileMode.Create))
            {
                upload.CopyTo(stream);
            }
            var url = $"{"/MyImages/"}{fileName}";
            return Json(new { uploaded = true, url });
        }
    }
}
