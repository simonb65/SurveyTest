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
        
        private SurveyTestEntities db = new SurveyTestEntities();

        //
        // GET: /Admin/Question/

        public ViewResult Index()
        {
            //var question_def = db.question_def.Include(q => q.QuestionFormat);
            //return View(question_def.ToList());

            var questions = _repo.ListQuestions();
            return View(questions);
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
        [HttpGet]
        public ActionResult Create()
        {
            LoadQuestionFormats();
            var model = new CreateQuestionModel();

            return View(model);
        } 

        //
        // POST: /Admin/Question/Create
        private void LoadQuestionFormats()
        {
            var formats = _repo.ListQuestionFormats();
            ViewBag.QuestionFormats = new SelectList(formats, "item1", "item2");
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
        
        //
        // GET: /Admin/Question/Edit/5
 
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