using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopLearn.Core.DTOs;
using TopLearn.Core.Services.Interfaces;

namespace TopLearn.Web.Pages.Admin.Users
{
    public class ListDeleteUsersModel : PageModel
    {
        private IUserService _userservice;

        public ListDeleteUsersModel(IUserService userservice)
        {
            _userservice = userservice;
        }
        public UserForAdminViewModel UserForAdminViewModel { get; set; }
        public void OnGet(int pageId = 1, string filterUsername = "", string filterEmail = "")
        {
            UserForAdminViewModel = _userservice.GetDeleteUsers(pageId, filterEmail, filterUsername);
        }

    }
}
