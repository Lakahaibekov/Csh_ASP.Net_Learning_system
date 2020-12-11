using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CourseProject.Models
{
    public class LessonListViewModel
    {
        public IEnumerable<Lesson> Lessons { get; set; }
        public SelectList Teachers { get; set; }

    }
}