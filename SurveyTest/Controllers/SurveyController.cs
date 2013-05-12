﻿using System;
using System.Linq;
using System.Web.Mvc;
using System.Diagnostics;

using SurveyTest.Models;
using SurveyTest.Repository;

namespace SurveyTest.Controllers
{
    public class SurveyController : Controller
    {
        private readonly ISurveyRepository _surveyRepository;

        public SurveyController(ISurveyRepository surveyRepository)
        {
            _surveyRepository = surveyRepository;
        }

        [HttpGet]
        public ActionResult Index(int id)
        {
            var survey = _surveyRepository.GetSurvey(id);
            return View(survey);
        }

        [HttpPost]
        public ActionResult Index(int id, string save)
        {
            var survey = _surveyRepository.GetSurvey(id);

            if (!TryUpdateModel(survey))
            {
                return View(survey);
            }

            var surveySessKey = Guid.NewGuid().ToString();

            Session[surveySessKey] = survey;

            TempData["SurveySessKey"] = surveySessKey;

            return RedirectToAction("Thanks");
        }

        [HttpGet]
        public ActionResult Thanks()
        {
            var model = new SubmitSurveyModel { SurveySessKey = (string)TempData["SurveySessKey"] };

            return View(model);
        }

        [HttpPost]
        public ActionResult Thanks(SubmitSurveyModel model)
        {
            if (ModelState.IsValid)
            {
                var survey = (SurveyModel)Session[model.SurveySessKey];
                _surveyRepository.StoreSurveyResult(model, survey);

                return RedirectToAction("Index", "Home");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
    }
}
