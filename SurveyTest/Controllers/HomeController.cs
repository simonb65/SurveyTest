using SurveyTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
