using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace CourseProject.Models
{
    public class KundelikContext : DbContext
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Parent> Parents { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<Teacher> Teachers { get; set; }

    }
}