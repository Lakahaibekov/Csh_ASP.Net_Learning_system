using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Threading.Tasks;
using System.Data.Entity;
using CourseProject.Models;


namespace CourseProject.Controllers
{
    public class HomeController : Controller
    {
        private KundelikContext db = new KundelikContext();

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
        public bool IsAdmin()
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
        public bool IsParent()
        {
            if (Session["role"] != null)
            {
                string role = (string)Session["role"];
                if (role == "Parent")
                {
                    return true;
                }
            }
            return false;
        }
        public bool IsTeacher()
        {
            if (Session["role"] != null)
            {
                string role = (string)Session["role"];
                if (role == "Teacher")
                {
                    return true;
                }
            }
            return false;
        }
        public bool IsStudent()
        {
            if (Session["role"] != null)
            {
                string role = (string)Session["role"];
                if (role == "Student")
                {
                    return true;
                }
            }
            return false;
        }



        public ActionResult Index()
        {
            if (!isAuthenticate())
            {
                return Redirect("/Admin/LoginPage/?error");
            }
            
            return View("Index");
        }

        public ActionResult ShowGrades(int? lessonId, int? studentId)
        {
            if (!isAuthenticate())
            {
                return Redirect("/Admin/LoginPage/?error");
            }


            IQueryable<Student> students = db.Students.Include(s => s.Group);
            IQueryable<Lesson> lessons = db.Lessons.Include(s => s.Group).Include(s => s.Course);

            Lesson lesson = lessons.First(s => s.Id == lessonId);
            Student student = students.First(s => s.Id == studentId);


            IQueryable<Grade> grades = db.Grades.Include(g => g.Student).Include(g => g.Lesson);
            grades = grades.Where(g => g.StudentId == studentId);
            grades = grades.Where(g => g.LessonId == lessonId);

            ViewBag.lesson = lesson;
            ViewBag.student = student;

            return View(grades);

        }

        public ActionResult PutGrade(int? lessonId, int? studentId)
        {
            if (!isAuthenticate())
            {
                return Redirect("/Admin/LoginPage/?error");
            }
            if (!IsAdmin())
            {
                if (!IsTeacher())
                {
                    return Redirect("/Admin/LoginPage/?noPermission");
                }
            }


            ViewBag.StudentId = new SelectList(db.Students, "Id", "Name");
            ViewBag.LessonId = new SelectList(db.Lessons, "Id", "CourseId");

            IQueryable<Student> students = db.Students.Include(s => s.Group);
            IQueryable<Lesson> lessons = db.Lessons.Include(s => s.Group).Include(s => s.Course);

            Lesson lesson = lessons.First(s => s.Id == lessonId);
            Student student = students.First(s => s.Id == studentId);

            ViewBag.lesson = lesson;
            ViewBag.student = student;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> PutGrade([Bind(Include = "Id,Value,StudentId,LessonId,Date")] Grade grade)
        {
            if (!isAuthenticate())
            {
                return Redirect("/Admin/LoginPage/?error");
            }
            if (!IsAdmin())
            {
                if (!IsTeacher())
                {
                    return Redirect("/Admin/LoginPage/?noPermission");
                }
            }


            if (ModelState.IsValid)
            {
                db.Grades.Add(grade);
                await db.SaveChangesAsync();
                return RedirectToAction("TeacherPage");
            }

            ViewBag.StudentId = new SelectList(db.Students, "Id", "Name", grade.StudentId);
            ViewBag.LessonId = new SelectList(db.Lessons, "Id", "CourseId", grade.LessonId);
            return View(grade);
        }

        public async Task<ActionResult> LessonDetails(int? lessonId)
        {
            if (!isAuthenticate())
            {
                return Redirect("/Admin/LoginPage/?error");
            }


            if (lessonId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IQueryable<Student> students = db.Students.Include(s => s.Group);
            IQueryable<Lesson> lessons = db.Lessons.Include(s => s.Group).Include(s => s.Course);

            Lesson lesson = lessons.First(s => s.Id == lessonId);
            students = students.Where(s => s.GroupId == lesson.GroupId);

            if (students == null)
            {
                return HttpNotFound();
            }
            ViewBag.lessonId = lesson.Id;
            ViewBag.lesson = lesson;
            return View(students);
        }

        public ActionResult LoginPage()
        {

            return View();
        }

        public ActionResult TeacherPage(int? teacher)
        {
            if (!isAuthenticate())
            {
                return Redirect("/Admin/LoginPage/?error");
            }
            if (!IsAdmin())
            {
                if (!IsTeacher())
                {
                    return Redirect("/Admin/LoginPage/?noPermission");
                }
            }

            IQueryable<Lesson> lessons = db.Lessons.Include(s => s.Teacher).Include(s => s.Group).Include(s => s.Course);
            
            if (teacher != null && teacher != 0)
            {
                if (IsAdmin())
                {
                    lessons = lessons.Where(s => s.TeacherId == teacher);
                }
                else
                {
                    return Redirect("/Admin/LoginPage/?noPermission");
                }
            }
            else
            {
                IQueryable<Teacher> teachers0 = db.Teachers;
                Teacher teacher1 = null;
                string email = (string)Session["currentUser"];
                foreach (Teacher t in teachers0)
                {
                    if (t.Login == email)
                    {
                        teacher1 = t;
                        break;
                    }
                }
                if (teacher1 != null)
                {
                    lessons = lessons.Where(s => s.TeacherId == teacher1.Id);
                }
            }

            List<Teacher> teachers = db.Teachers.ToList();
            // устанавливаем начальный элемент, который позволит выбрать всех
            teachers.Insert(0, new Teacher { Name = "Все", Id = 0 });


            LessonListViewModel plvm = new LessonListViewModel
            {
                Lessons = lessons.Include(s => s.Teacher).Include(s => s.Group).Include(s => s.Course).ToList(),
                Teachers = new SelectList(teachers, "Id", "Name")
            };
            return View(plvm);
        }

        public ActionResult StudentPage(int? student)
        {
            if (!isAuthenticate())
            {
                return Redirect("/Admin/LoginPage/?error");
            }
            if (!IsParent())
            {
                IQueryable<Lesson> lessons = db.Lessons.Include(s => s.Group).Include(s => s.Course);
                IQueryable<Grade> grades = db.Grades.Include(s => s.Student).Include(s => s.Lesson.Course).Include(s => s.Lesson.Group);
                if (student != null && student != 0)
                {
                    if (!IsStudent())
                    {
                        if (!IsParent())
                        {
                            grades = grades.Where(s => s.StudentId == student);
                        }
                        else
                        {
                            return Redirect("/Admin/LoginPage/?noPermission");
                        }
                    }
                    else
                    {
                        return Redirect("/Admin/LoginPage/?noPermission");
                    }
                }
                else
                {
                    IQueryable<Student> students0 = db.Students;
                    Student student1 = null;
                    string email = (string)Session["currentUser"];
                    foreach (Student s in students0)
                    {
                        if (s.Login == email)
                        {
                            student1 = s;
                            break;
                        }
                    }
                    if (student1 != null)
                    {
                        grades = grades.Where(s => s.StudentId == student1.Id);
                    }
                }

                List<Student> students = db.Students.ToList();
                // устанавливаем начальный элемент, который позволит выбрать всех
                students.Insert(0, new Student { Name = "Все", Id = 0 });

                GradeListViewModel plvm = new GradeListViewModel
                {
                    Grades = grades.ToList(),
                    Students = new SelectList(students, "Id", "Name")
                };
                return View(plvm);
            }
            else
            {
                return Redirect("/Admin/LoginPage/?noPermission");
            }
        }

        public ActionResult ParentPage(int? parent)
        {
            if (!isAuthenticate())
            {
                return Redirect("/Admin/LoginPage/?error");
            }
            if (!IsAdmin())
            {
                if (!IsParent())
                {
                    return Redirect("/Admin/LoginPage/?noPermission");
                }
            }
            IQueryable<Grade> grades = db.Grades.Include(s => s.Student).Include(s => s.Lesson.Course).Include(s => s.Lesson.Group);
            IQueryable<Student> students1 = db.Students.Include(s => s.Parent);
            if (parent != null && parent != 0)
            {
                if (IsAdmin())
                {
                    Student student = students1.First(s => s.ParentId == parent);
                    grades = grades.Where(s => s.StudentId == student.Id);
                }
                else
                {
                    return Redirect("/Admin/LoginPage/?noPermission");
                }
            }
            else
            {
                IQueryable<Parent> parents1 = db.Parents;
                Parent parent1 = null;
                string email = (string)Session["currentUser"];
                foreach (Parent p in parents1)
                {
                    if (p.Login == email)
                    {
                        parent1 = p;
                        break;
                    }
                }
                if (parent1 != null)
                {
                    Student student = students1.First(s => s.ParentId == parent1.Id);
                    grades = grades.Where(s => s.Student.ParentId == parent1.Id);
                    //grades = grades.Where(s => s.StudentId == student.Id);
                }
            }

            List<Parent> parents = db.Parents.ToList();
            // устанавливаем начальный элемент, который позволит выбрать всех
            parents.Insert(0, new Parent { Name = "Все", Id = 0 });

            GradeListViewModel plvm = new GradeListViewModel
            {
                Grades = grades.Include(s => s.Student).Include(s => s.Lesson.Course).Include(s => s.Lesson.Group).ToList(),
                Parents = new SelectList(parents, "Id", "Name")
            };
            return View(plvm);
        }


        /*public ActionResult ParentPage(int? parent)
        {
            IQueryable<Grade> grades = db.Grades.Include(s => s.SchoolKid).Include(s => s.Subject);
            IQueryable<SchoolKid> schoolKids1 = db.SchoolKids.Include(s => s.Mother).Include(s => s.Father);
            if (parent != null && parent != 0)
            {
                SchoolKid schoolKid = schoolKids1.First(s => s.MotherId == parent);
                if (schoolKid == null)
                {
                    schoolKid = schoolKids1.First(s => s.FatherId == parent);
                }
                grades = grades.Where(s => s.SchoolKidId == schoolKid.Id);
            }

            List<Parent> parents = db.Parents.ToList();
            // устанавливаем начальный элемент, который позволит выбрать всех
            parents.Insert(0, new Parent { Name = "Все", Id = 0 });

            GradeListViewModel plvm = new GradeListViewModel
            {
                Grades = grades.Include(s => s.SchoolKid).Include(s => s.Subject).ToList(),
                Parents = new SelectList(parents, "Id", "Name")
            };
            return View(plvm);
        }*/

        //
        /*

        [HttpPost]
        public ActionResult DeleteSchoolKid(int id)
        {
            SchoolKid s = db.SchoolKids.Find(id);
            if (s == null)
            {
                return HttpNotFound();
            }
            db.SchoolKids.Remove(s);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost, ActionName("DeleteSchoolKid")]
        public void DeleteSchoolKidConfirmed(int id)
        {
            SchoolKid s = db.SchoolKids.Find(id);
            db.SchoolKids.Remove(s);
            db.SaveChanges();
        }

        [HttpGet]
        public ActionResult CreateSchoolKid()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateSchoolKid(SchoolKid schoolKid)
        {
            db.SchoolKids.Add(schoolKid);
            db.SaveChanges();

            return RedirectToAction("Index");
        }



        [HttpGet]
        public ActionResult KidDetails(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            SchoolKid schoolKid = db.SchoolKids.Find(id);
            if (schoolKid != null)
            {
                return View(schoolKid);//todo
            }
            return HttpNotFound();
        }

        [HttpPost]
        public ActionResult KidDetails(SchoolKid schoolKid)
        {
            db.Entry(schoolKid).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        [HttpGet]
        public ActionResult PutGrade(int id)
        {
            if (id > 10) {
                return Redirect("/Home/Index");
            }
            ViewBag.kidId = id;
            return View();
        }

        [HttpPost]
        public string PutGrade(int kidId, string SubjectName, int grade)
        {
            // добавляем информацию о покупке в базу данных
            Grade gr = new Grade();
            string name = "FFF";
            IEnumerable<SchoolKid> schoolKids = db.SchoolKids;
            foreach (SchoolKid kid in schoolKids) {
                if (kid.Id.Equals(kidId)) {
                    name = kid.Name;
                }
            }
            gr.KidName = name;
            gr.grade = grade;
            gr.SubjectName = SubjectName;
            db.Grades.Add(gr);
            // сохраняем в бд все изменения
            db.SaveChanges();
            return "Name:," + gr.KidName + ", Subject:" + gr.SubjectName + ", Grade: " + gr.grade;
        }
        */


    }
}