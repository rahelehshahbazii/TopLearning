using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TopLearn.DataLayer.Entities.Course
{
   public class CourseStatus
    {
        [Key]
        public int StatusId { get; set; }

        [MaxLength(150)]
        [Required]
        public string StatusTitle { get; set; }

        public List<Course> Courses { get; set; }
    }
}
