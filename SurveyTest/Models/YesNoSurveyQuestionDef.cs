using System;
using System.Web.Mvc;

namespace SurveyTest.Models
{
    public class YesNoSurveyQuestionDef : MultiChoiceQuestionDef
    {
        public YesNoSurveyQuestionDef()
        {
            QuestionTexts = new[] { "Yes", "No" };
        }

        public override string FormatType { get { return "YesNo"; } }

        public override QuestionResult GetResult(IValueProvider provider)
        {
            string value;
            if (TryGetValue(provider, GroupName, out value))
            {
                var idx = int.Parse(value.Substring(ResultPrefix.Length));
                var answer = (idx == 0);

                return new QuestionResult(this) { Answer = answer.ToString() };
            }

            return null;
        }
    }
}