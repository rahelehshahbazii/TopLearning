﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TopLearn.DataLayer.Entities.User;

namespace TopLearn.DataLayer.Entities.Order
{
    public class Discount
    {
        [Key]
        public int DiscountId { get; set; }
        [Display(Name="کد")] 
        [Required(ErrorMessage = "لطفا {0}را وارد گنید")]
        [MaxLength(150)]
        public string DiscountCode { get; set; }

        [Display(Name = "درصد")]
        [Required(ErrorMessage = "لطفا {0}را وارد گنید")]
        public int DiscountPercent { get; set; }
        public int? UsableCount { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public List<UserDiscountCode> UserDicountCodes { get; set; }
    }
}
