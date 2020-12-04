using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CourseProject.Models
{
    public class StudentListViewModel
    {
        public IEnumerable<Student> Students { get; set; }
        public SelectList Groups { get; set; }

    }
}