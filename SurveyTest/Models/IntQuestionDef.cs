using System;
using System.Web.Mvc;

namespace SurveyTest.Models
{
    public class IntQuestionDef : TextQuestionDef
    {
        public override QuestionResult GetResult(IValueProvider provider)
        {
            int value = 0;
            TryGetValue(provider, ResultName, out value);

            return new QuestionResult { Answer = value.ToString(), Value = 0};
        }
    }
}