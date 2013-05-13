using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

using SurveyTest.Repository;
using SurveyTest.Models;
using SurveyTest.Areas.Admin.Models;

namespace SurveyTest.Areas.Admin.Controllers
{ 
    public class QuestionController : Controller
    {
        private readonly ISurveyRepository _repo;

        public QuestionController(ISurveyRepository repo)
        {
            _repo = repo;
        }
        
        // GET: /Admin/Question/
        [HttpGet]
        public ViewResult Index()
        {
            var questions = _repo.ListQuestions();
            return View(questions);
        }

        private void LoadQuestionFormats()
        {
            var formats = _repo.ListQuestionFormats();
            ViewBag.QuestionFormats = new SelectList(formats, "item1", "item2");
        }

        [HttpGet]
        public ActionResult Create()
        {
            LoadQuestionFormats();
            var model = new CreateQuestionModel();

            return View(model);
        } 

        [HttpPost]
        public ActionResult Create(CreateQuestionModel model)
        {
            if (ModelState.IsValid)
            {
                _repo.SaveNewQuestionDef(model.Name, model.FormatId, model.PromptText);
                return RedirectToAction("Index");  
            }

            LoadQuestionFormats();

            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var qd = _repo.GetQuestion(id);
            LoadQuestionFormats();
       
            return View(qd);
        }

        [HttpPost]
        public ActionResult Edit(QuestionDef model)
        {
            if (ModelState.IsValid)
            {
                _repo.UpdateQuestionDef(model);
                return RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult AddOption(int id, string text, int value)
        {
            var qd = (MultiChoiceQuestionDef)_repo.GetQuestion(id);

            qd.Questions.Add(new MultiChoiceQuestionDef.QuestionOption { Text = text, Value = value, Order = qd.Questions.Count + 1 });

            _repo.UpdateQuestionDef(qd);

            return Json(new { result = true });
        }

        [HttpPost]
        public ActionResult MoveOption(int id, int optIdx, int direction)
        {
            // First and move up - ignore
            if ((optIdx == 0) && (direction < 0))
                return Json(new { result = true });

            var qd = (MultiChoiceQuestionDef)_repo.GetQuestion(id);

            // First and move down - ignore
            if ((optIdx == (qd.Questions.Count - 1)) && (direction > 0))
                return Json(new { result = true });


            // Swap order
            var q = qd.Questions[optIdx];
            var p = qd.Questions[optIdx + direction];

            var tmp = q.Order;
            q.Order = p.Order;
            p.Order = tmp;

            qd.Questions = qd.Questions.OrderBy(x => x.Order).ToList();

            _repo.UpdateQuestionDef(qd);

            return Json(new { result = true });
        }

        [HttpPost]
        public ActionResult RemoveOption(int id, int optIdx)
        {
            var qd = (MultiChoiceQuestionDef)_repo.GetQuestion(id);

            qd.Questions.RemoveAt(optIdx);

            _repo.UpdateQuestionDef(qd);

            return Json(new { result = true });
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var qd = _repo.GetQuestion(id);
            return View(qd);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            _repo.DeleteQuestion(id);

            return RedirectToAction("Index");
        }
    }
}