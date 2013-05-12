using System;
using System.Web.Mvc;

namespace SurveyTest.Models
{
    public class IntQuestionDef : TextQuestionDef
    {
        public override string FormatType { get { return "Int"; } }

        public override QuestionResult GetResult(IValueProvider provider)
        {
            int value;
            return (TryGetValue(provider, ResultName, out value))
                ? new QuestionResult(this) { Answer = value.ToString() }
                : null;
        }
    }
}