using System;
using System.Web.Mvc;

namespace SurveyTest.Models
{
    public class HeaderQuestionDef : SurveyQuestionDef
    {
        public HeaderQuestionDef(int id, string promptText)
            : base(id, promptText)
        {
        }

        public override bool HasResult { get { return false; } }
        public override QuestionResult GetResult(IValueProvider provider) { return null; }
    }
}