using CourseProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CourseProject.Controllers
{
    public class AdminController : Controller
    {
        KundelikContext db = new KundelikContext();
        public bool isAuthenticate()
        {
            if (Session["currentUser"] != null)
            {
                if (Session["role"] != null)
                {
                    return true;
                }
            }
            return false;
        }

        public bool HavePermission()
        {
            if (Session["role"] != null)
            {
                string role = (string)Session["role"];
                if (role == "Admin")
                {
                    return true;
                }
            }
            return false;
        }

        // GET: Admin
        public ActionResult Index()
        {
            if (!isAuthenticate())
            {
                return Redirect("/Admin/LoginPage/?notAuthenticate");
            }
            if (!HavePermission())
            {
                return Redirect("/Admin/LoginPage/?noPermission");
            }
            View(db.Courses);
            return View("Index");
        }

        public ActionResult LoginPage()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LoginPage(string inputEmail, string inputPassword)
        {
            IQueryable<Student> students = db.Students;
            foreach (Student student in students)
            {
                if (student.Login == inputEmail)
                {
                    if (student.Password == inputPassword)
                    {
                        Session["currentUser"] = inputEmail;
                        Session["role"] = "Student";
                        return Redirect("/Home/StudentPage");
                    }
                }
            }
            IQueryable<Parent> parents = db.Parents;
            foreach (Parent parent in parents)
            {
                if (parent.Login == inputEmail)
                {
                    if (parent.Password == inputPassword)
                    {
                        Session["currentUser"] = inputEmail;
                        Session["role"] = "Parent";
                        return Redirect("/Home/ParentPage");
                    }
                }
            }
            IQueryable<Teacher> teachers = db.Teachers;
            foreach (Teacher teacher in teachers)
            {
                if (teacher.Login == inputEmail)
                {
                    if (teacher.Password == inputPassword)
                    {
                        Session["currentUser"] = inputEmail;
                        Session["role"] = "Teacher";
                        return Redirect("/Home/TeacherPage");
                    }
                }
            }
            if (inputEmail == "admin") {
                if (inputPassword == "pass123") {
                    Session["currentUser"] = inputEmail;
                    Session["role"] = "Admin";
                    return Redirect("/Courses/Index");
                }
            }
            return View();
        }
    }
}