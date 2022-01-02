using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopLearn.Core.Services.Interfaces;
using System.Globalization;
using TopLearn.DataLayer.Entities.Order;

namespace TopLearn.Web.Pages.Admin.Discount
{
    public class CreateDiscountModel : PageModel
    {
        private IOrderService _orderservice;
        public CreateDiscountModel(IOrderService orderservice)
        {
            _orderservice = orderservice;
        }
        
        [BindProperty]
        public TopLearn.DataLayer.Entities.Order.Discount Discount { get; set; }

        public void OnGet()
        {
        }
        public IActionResult OnPost(string stDate ="" ,string edDate="")
        {
            if (stDate != "")
            { 
                string[] std = stDate.Split('/');
               
                Discount.StartDate = new System.DateTime(int.Parse(std[0]),
                    int.Parse(std[1]),
                    int.Parse(std[2]),
                    new PersianCalendar()
                                                        );
            }
            if (edDate !="")
            {
                string[] edd = edDate.Split('/');

                Discount.EndDate = new System.DateTime(int.Parse(edd[0]),
                    int.Parse(edd[1]),
                    int.Parse(edd[2]),
                    new PersianCalendar()
                                                        );
            }
            if (!ModelState.IsValid)
                return Page();
            _orderservice.AddDiscount(Discount);
            return RedirectToPage("Index");
        
        }

    }
}
