using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CourseProject.Models
{
    public class Grade
    {
        public int Id { get; set; }
        public int Value { get; set; }
        public int? StudentId { get; set; }
        public Student Student { get; set; }
        public int? LessonId { get; set; }
        public Lesson Lesson { get; set; }

        public DateTime Date { get; set; }
    }
}