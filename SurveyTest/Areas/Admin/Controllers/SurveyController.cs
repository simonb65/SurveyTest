using System;
using System.Web.Mvc;

using SurveyTest.Models;
using SurveyTest.Repository;

namespace SurveyTest.Areas.Admin.Controllers
{ 
    public class SurveyController : Controller
    {

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
        
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var survey = _repo.GetSurvey(id);
            return View(survey);
        }

        [HttpPost]
        public ActionResult Edit(SurveyModel survey)
        {
            if (ModelState.IsValid)
            {
                _repo.SaveSurvey(survey);
                return RedirectToAction("Index");
            }
            return View(survey);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var survey = _repo.GetSurvey(id);
            return View(survey);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            _repo.DeleteSurvey(id);
            return RedirectToAction("Index");
        }
    }
}