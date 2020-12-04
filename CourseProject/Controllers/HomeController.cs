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

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ShowGrades(int? lessonId, int? studentId)
        {
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
            if (ModelState.IsValid)
            {
                db.Grades.Add(grade);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.StudentId = new SelectList(db.Students, "Id", "Name", grade.StudentId);
            ViewBag.LessonId = new SelectList(db.Lessons, "Id", "CourseId", grade.LessonId);
            return View(grade);
        }

        public async Task<ActionResult> LessonDetails(int? lessonId)
        {
            if (lessonId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IQueryable<Student> students = db.Students.Include(s => s.Group);
            IQueryable<Lesson> lessons = db.Lessons.Include(s => s.Group);

            Lesson lesson = lessons.First(s => s.Id == lessonId);
            students = students.Where(s => s.GroupId == lesson.GroupId);

            if (students == null)
            {
                return HttpNotFound();
            }
            ViewBag.lessonId = lesson.Id;
            return View(students);
        }

        public ActionResult LoginPage()
        {

            return View();
        }

        public ActionResult TeacherPage(int? teacher)
        {
            IQueryable<Lesson> lessons = db.Lessons.Include(s => s.Teacher).Include(s => s.Group).Include(s => s.Course);
            if (teacher != null && teacher != 0)
            {
                lessons = lessons.Where(s => s.TeacherId == teacher);
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
            IQueryable<Lesson> lessons = db.Lessons.Include(s => s.Group).Include(s => s.Course);
            IQueryable<Grade> grades = db.Grades.Include(s => s.Student).Include(s => s.Lesson.Course).Include(s => s.Lesson.Group);
            if (student != null && student != 0)
            {
                grades = grades.Where(s => s.StudentId == student);
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

        public ActionResult ParentPage(int? parent)
        {
            IQueryable<Grade> grades = db.Grades.Include(s => s.Student).Include(s => s.Lesson.Course).Include(s => s.Lesson.Group);
            IQueryable<Student> students1 = db.Students.Include(s => s.Parent);
            if (parent != null && parent != 0)
            {
                Student student = students1.First(s => s.ParentId == parent);
                grades = grades.Where(s => s.StudentId == student.Id);
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