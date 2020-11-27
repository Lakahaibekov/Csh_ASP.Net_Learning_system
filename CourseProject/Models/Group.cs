using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CourseProject.Models
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? AdviserId { get; set; }
        public Teacher Adviser { get; set; }

        public ICollection<Student> Students { get; set; }
        public Group()
        {
            Students = new List<Student>();
        }
    }
}