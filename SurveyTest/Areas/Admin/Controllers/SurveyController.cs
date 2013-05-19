using System;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;

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
            var questions = _repo.ListQuestions();
            var survey = _repo.GetSurvey(id);
            ViewBag.AvailableQuestions = questions
                .Where(x => !survey.Questions.Any(q => q.Question.Id == x.Id))
                .OrderBy(x => x.Name)
                .Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() })
                .ToList();

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

        [HttpPost]
        public ActionResult AddQuestions(int id, string questionIds)
        {
            var survey = _repo.GetSurvey(id);
            
            if (string.IsNullOrEmpty(questionIds) || (survey == null))
                return Json(new { result = false });

            var ids = questionIds.Split(',').Select(x => int.Parse(x));

            var questions = _repo.ListQuestions().Where(q => ids.Contains(q.Id));

            var order = survey.Questions.Any() ? survey.Questions.Max(x => x.Order) + 1 : 1;
            foreach (var question in questions)
            {
                survey.Questions.Add(new SurveyQuestion { Question = question, Order = ++order, Mandatory = true });
            }

            _repo.SaveSurvey(survey);

            return Json(new { result = true });
        }

        [HttpPost]
        public ActionResult RemoveQuestion(int id, int questionIdx)
        {
            var survey = _repo.GetSurvey(id);

            survey.Questions.RemoveAt(questionIdx);

            _repo.SaveSurvey(survey);

            return Json(new { result = true });
        }
        [HttpPost]
        public ActionResult MoveQuestion(int id, int questionIdx, int direction)
        {
            var survey = _repo.GetSurvey(id);

            var q = survey.Questions[questionIdx];
            var p = survey.Questions[questionIdx + direction];

            var tmp = q.Order;
            q.Order = p.Order;
            p.Order = tmp;

            _repo.SaveSurvey(survey);
            
            
            return Json(new { result = true });
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