using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopLearn.Core.DTOs;
using TopLearn.Core.Security;
using TopLearn.Core.Services.Interfaces;

namespace TopLearn.Web.Pages.Admin.Users
{
    [PermissionChecker(5)]
    public class DeleteUserModel : PageModel
    {
        private IUserService _userservice;
        public DeleteUserModel(IUserService userservice)
        {
            _userservice = userservice;
        }

        public InformationUserViewModel InformationUserViewModel { get; set; }

        public void OnGet(int id)
        {
            ViewData["UserId"] = id;
            InformationUserViewModel = _userservice.GetUserInformation(id);
        }

        public IActionResult OnPost(int UserId)
        {
            _userservice.DeleteUser(UserId);
            return RedirectToPage("Index");
        }
         



    }
}
