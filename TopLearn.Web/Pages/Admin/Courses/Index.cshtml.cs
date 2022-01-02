using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using TopLearn.Core.DTOs.Course;
using TopLearn.Core.Services.Interfaces;
namespace TopLearn.Web.Pages.Admin.Courses
{
    public class IndexModel : PageModel
    {

        private ICourseService _courseService;

        public IndexModel(ICourseService courseService)
        {
            _courseService = courseService;
        }
        public List<ShowCourseForAdminViewModel> ListCoures  { get; set; }

        public void OnGet()
        {
            ListCoures = _courseService.GetCoursesForAdmin();


        }
    }
}
