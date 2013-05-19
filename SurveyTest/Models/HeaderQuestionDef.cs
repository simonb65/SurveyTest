using System;
using System.Web.Mvc;

namespace SurveyTest.Models
{
    public class HeaderQuestionDef : QuestionDef
    {
        public override bool HasResult { get { return false; } }
        public override string GetResult(IValueProvider provider) { return null; }

        public override string FormatType { get { return "Header"; } }
    }
}