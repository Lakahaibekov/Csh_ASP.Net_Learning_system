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
    public class LessonsController : Controller
    {
        private KundelikContext db = new KundelikContext();

        // GET: Lessons
        public async Task<ActionResult> Index()
        {
            var lessons = db.Lessons.Include(l => l.Course).Include(l => l.Group).Include(l => l.Teacher);
            return View(await lessons.ToListAsync());
        }

        // GET: Lessons/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lesson lesson = await db.Lessons.FindAsync(id);
            if (lesson == null)
            {
                return HttpNotFound();
            }
            return View(lesson);
        }

        // GET: Lessons/Create
        public ActionResult Create()
        {
            ViewBag.CourseId = new SelectList(db.Courses, "Id", "Name");
            ViewBag.GroupId = new SelectList(db.Groups, "Id", "Name");
            ViewBag.TeacherId = new SelectList(db.Teachers, "Id", "Login");
            return View();
        }

        // POST: Lessons/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,CourseId,TeacherId,GroupId")] Lesson lesson)
        {
            if (ModelState.IsValid)
            {
                db.Lessons.Add(lesson);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.CourseId = new SelectList(db.Courses, "Id", "Name", lesson.CourseId);
            ViewBag.GroupId = new SelectList(db.Groups, "Id", "Name", lesson.GroupId);
            ViewBag.TeacherId = new SelectList(db.Teachers, "Id", "Login", lesson.TeacherId);
            return View(lesson);
        }

        // GET: Lessons/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lesson lesson = await db.Lessons.FindAsync(id);
            if (lesson == null)
            {
                return HttpNotFound();
            }
            ViewBag.CourseId = new SelectList(db.Courses, "Id", "Name", lesson.CourseId);
            ViewBag.GroupId = new SelectList(db.Groups, "Id", "Name", lesson.GroupId);
            ViewBag.TeacherId = new SelectList(db.Teachers, "Id", "Login", lesson.TeacherId);
            return View(lesson);
        }

        // POST: Lessons/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,CourseId,TeacherId,GroupId")] Lesson lesson)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lesson).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CourseId = new SelectList(db.Courses, "Id", "Name", lesson.CourseId);
            ViewBag.GroupId = new SelectList(db.Groups, "Id", "Name", lesson.GroupId);
            ViewBag.TeacherId = new SelectList(db.Teachers, "Id", "Login", lesson.TeacherId);
            return View(lesson);
        }

        // GET: Lessons/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lesson lesson = await db.Lessons.FindAsync(id);
            if (lesson == null)
            {
                return HttpNotFound();
            }
            return View(lesson);
        }

        // POST: Lessons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Lesson lesson = await db.Lessons.FindAsync(id);
            db.Lessons.Remove(lesson);
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
