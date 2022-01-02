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
    [PermissionChecker(2)]
    public class IndexModel : PageModel
    {
        private IUserService _userservice;

        public IndexModel(IUserService userservice)
        {
            _userservice = userservice;
        }
        public UserForAdminViewModel UserForAdminViewModel { get; set; }
        public void OnGet(int pageId=1,string filterUsername="",string filterEmail="")
        {
            UserForAdminViewModel = _userservice.GetUsers(pageId, filterEmail, filterUsername);
        }

    }
}
