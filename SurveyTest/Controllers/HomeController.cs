using System;

using System.Web.Mvc;

using SurveyTest.Repository;

namespace SurveyTest.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISurveyRepository _surveyRepository;

        public HomeController(ISurveyRepository surveyRepository)
        {
            _surveyRepository = surveyRepository;
        }

        public ActionResult Index()
        {
            ViewBag.Message = "Welcome to Test Survey System";

            var surveys = _surveyRepository.ListSurveys();

            return View(surveys);
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
