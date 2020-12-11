using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CourseProject.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Lesson> Lessons { get; set; }
        public Course()
        {
            Lessons = new List<Lesson>();
        }
    }
}