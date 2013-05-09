using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SurveyTest.Repository;

namespace SurveyTest.Areas.Admin.Controllers
{ 
    public class QuestionController : Controller
    {
        private SurveyTestEntities db = new SurveyTestEntities();

        //
        // GET: /Admin/Question/

        public ViewResult Index()
        {
            var question_def = db.question_def.Include(q => q.QuestionFormat);
            return View(question_def.ToList());
        }

        //
        // GET: /Admin/Question/Details/5

        public ViewResult Details(int id)
        {
            question_def question_def = db.question_def.Find(id);
            return View(question_def);
        }

        //
        // GET: /Admin/Question/Create

        public ActionResult Create()
        {
            ViewBag.question_format_id = new SelectList(db.question_format, "question_format_id", "question_format1");
            return View();
        } 

        //
        // POST: /Admin/Question/Create

        [HttpPost]
        public ActionResult Create(question_def question_def)
        {
            if (ModelState.IsValid)
            {
                db.question_def.Add(question_def);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            ViewBag.question_format_id = new SelectList(db.question_format, "question_format_id", "question_format1", question_def.question_format_id);
            return View(question_def);
        }
        
        //
        // GET: /Admin/Question/Edit/5
 
        public ActionResult Edit(int id)
        {
            question_def question_def = db.question_def.Find(id);
            ViewBag.question_format_id = new SelectList(db.question_format, "question_format_id", "question_format1", question_def.question_format_id);
            return View(question_def);
        }

        //
        // POST: /Admin/Question/Edit/5

        [HttpPost]
        public ActionResult Edit(question_def question_def)
        {
            if (ModelState.IsValid)
            {
                db.Entry(question_def).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.question_format_id = new SelectList(db.question_format, "question_format_id", "question_format1", question_def.question_format_id);
            return View(question_def);
        }

        //
        // GET: /Admin/Question/Delete/5
 
        public ActionResult Delete(int id)
        {
            question_def question_def = db.question_def.Find(id);
            return View(question_def);
        }

        //
        // POST: /Admin/Question/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            question_def question_def = db.question_def.Find(id);
            db.question_def.Remove(question_def);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}