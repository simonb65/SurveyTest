using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace SurveyTest.Models
{
    public class SubmitSurveyModel
    {
        public string SurveySessKey { get; set; }

        [Description("Persons Name")] 
        public string Name { get; set; }

        [Description("Persons Email")]
        public string Email { get; set; }
    }
}