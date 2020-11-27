using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CourseProject.Models;

namespace CourseProject.Controllers
{
    public class GradesController : Controller
    {
        private KundelikContext db = new KundelikContext();

        // GET: Grades
        public async Task<ActionResult> Index()
        {
            var grades = db.Grades.Include(g => g.Lesson).Include(g => g.Student);
            return View(await grades.ToListAsync());
        }

        // GET: Grades/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Grade grade = await db.Grades.FindAsync(id);
            if (grade == null)
            {
                return HttpNotFound();
            }
            return View(grade);
        }

        // GET: Grades/Create
        public ActionResult Create()
        {
            ViewBag.LessonId = new SelectList(db.Lessons, "Id", "Id");
            ViewBag.StudentId = new SelectList(db.Students, "Id", "Login");
            return View();
        }

        // POST: Grades/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Value,StudentId,LessonId,Date")] Grade grade)
        {
            if (ModelState.IsValid)
            {
                db.Grades.Add(grade);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.LessonId = new SelectList(db.Lessons, "Id", "Id", grade.LessonId);
            ViewBag.StudentId = new SelectList(db.Students, "Id", "Login", grade.StudentId);
            return View(grade);
        }

        // GET: Grades/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Grade grade = await db.Grades.FindAsync(id);
            if (grade == null)
            {
                return HttpNotFound();
            }
            ViewBag.LessonId = new SelectList(db.Lessons, "Id", "Id", grade.LessonId);
            ViewBag.StudentId = new SelectList(db.Students, "Id", "Login", grade.StudentId);
            return View(grade);
        }

        // POST: Grades/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Value,StudentId,LessonId,Date")] Grade grade)
        {
            if (ModelState.IsValid)
            {
                db.Entry(grade).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.LessonId = new SelectList(db.Lessons, "Id", "Id", grade.LessonId);
            ViewBag.StudentId = new SelectList(db.Students, "Id", "Login", grade.StudentId);
            return View(grade);
        }

        // GET: Grades/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Grade grade = await db.Grades.FindAsync(id);
            if (grade == null)
            {
                return HttpNotFound();
            }
            return View(grade);
        }

        // POST: Grades/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Grade grade = await db.Grades.FindAsync(id);
            db.Grades.Remove(grade);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
