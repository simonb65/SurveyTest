using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SurveyTest.Models
{
    public class SurveyQuestion
    {
        public int Id { get; set; }
        public QuestionDef Question { get; set; }
        public int Order { get; set; }
        public bool Mandatory { get; set; }
    }
}