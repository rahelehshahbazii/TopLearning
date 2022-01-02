using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopLearn.Core.DTOs;
using TopLearn.Core.Security;
using TopLearn.Core.Services.Interfaces;

namespace TopLearn.Web.Pages.Admin.Users
{
    [PermissionChecker(4)]
    public class EditUserModel : PageModel
    {
        private IUserService _userservice;
        private IPermissionService _permissionService;
        public EditUserModel(IUserService userservice, IPermissionService permissionService)
        {
            _userservice = userservice;
            _permissionService = permissionService;
        }
        
        
        [BindProperty]
        public EditUserViewModel EditUserViewModel { get; set; }
        public void OnGet(int id)
        {
            EditUserViewModel = _userservice.GetUserForShowInEditMode(id);
            ViewData["Roles"] = _permissionService.GetRoles();
        }
        public IActionResult OnPost(List<int> SelectedRoles)
            {
               if(!ModelState.IsValid)
                {
                     return Page();
                }
            _userservice.EditUserFromAdmin(EditUserViewModel);

            //Edit Roles
            _permissionService.EditRolesUser(EditUserViewModel.UserId, SelectedRoles);
            return RedirectToPage("Index");
            }
    }
}
