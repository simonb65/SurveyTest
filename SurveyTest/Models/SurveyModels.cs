﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SurveyTest.Models
{
    public class SurveyModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IList<SurveyQuestion> Questions { get; set; }
    }

    // Separate class for model binding.
    public class SurveyRunModel : SurveyModel
    {
        public IDictionary<SurveyQuestion, SurveyQuestionResult> Answers { get; set; }

        public int Score()
        {
            return Answers.Sum(x => x.Value.Value);
        }
    }
}