using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CourseProject.Models
{
    public class Lesson
    {
        public int Id { get; set; }

        public int? CourseId { get; set; }
        public Course Course { get; set; }

        public int? TeacherId { get; set; }
        public Teacher Teacher { get; set; }

        public int? GroupId { get; set; }
        public Group Group { get; set; }
    }
}