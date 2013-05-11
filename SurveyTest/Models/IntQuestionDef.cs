using System;
using System.Web.Mvc;

namespace SurveyTest.Models
{
    public class IntQuestionDef : TextQuestionDef
    {
        public IntQuestionDef(int id, string promptText, bool singleLine = true)
            : base(id, promptText, singleLine)
        {
        }

        public override string FormatType { get { return GetType().BaseType.Name; } }

        public override QuestionResult GetResult(IValueProvider provider)
        {
            int value;
            return (TryGetValue(provider, ResultName, out value))
                ? new QuestionResult(this) { Answer = value.ToString() }
                : null;
        }
    }
}