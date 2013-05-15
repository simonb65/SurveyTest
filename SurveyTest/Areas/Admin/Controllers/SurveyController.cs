using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using SurveyTest.Models;
using SurveyTest.Repository;

namespace SurveyTest.Areas.Admin.Controllers
{ 
    public class SurveyController : Controller
    {
        private SurveyTestEntities db = new SurveyTestEntities();


        private readonly ISurveyRepository _repo;
        public SurveyController(ISurveyRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public ViewResult Index()
        {
            var surveys = _repo.ListSurveys();
            return View(surveys);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        } 

        [HttpPost]
        public ActionResult Create(SurveyModel survey)
        {
            if (ModelState.IsValid)
            {
                _repo.SaveSurvey(survey);
                return RedirectToAction("Index");  
            }

            return View(survey);
        }
        
        //
        // GET: /Admin/Survey/Edit/5
 
        public ActionResult Edit(int id)
        {
            survey survey = db.surveys.Find(id);
            return View(survey);
        }

        //
        // POST: /Admin/Survey/Edit/5

        [HttpPost]
        public ActionResult Edit(survey survey)
        {
            if (ModelState.IsValid)
            {
                db.Entry(survey).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(survey);
        }

        //
        // GET: /Admin/Survey/Delete/5
 
        public ActionResult Delete(int id)
        {
            survey survey = db.surveys.Find(id);
            return View(survey);
        }

        //
        // POST: /Admin/Survey/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            survey survey = db.surveys.Find(id);
            db.surveys.Remove(survey);
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