using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;

namespace SurveyTest.Models
{
    public class SubmitSurveyModel
    {
        public string SurveySessKey { get; set; }

        public int SurveyScore { get; set; }

        [Required]
        [Display(Name = "Survey name")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email address")]
        public string Email { get; set; }
    }
}