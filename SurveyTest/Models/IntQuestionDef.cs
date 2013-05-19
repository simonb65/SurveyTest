using System;
using System.Web.Mvc;

namespace SurveyTest.Models
{
    public class IntQuestionDef : TextQuestionDef
    {
        public override string GetResult(IValueProvider provider)
        {
            int value;
            return (TryGetValue(provider, ResultName, out value))
                ? value.ToString()
                : null;
        }
    }
}