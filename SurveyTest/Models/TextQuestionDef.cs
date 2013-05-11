using System;
using System.Web.Mvc;

namespace SurveyTest.Models
{
    public class TextQuestionDef : SurveyQuestionDef
    {
        public TextQuestionDef(int id, string promptText, bool singleLine = true)
            : base(id, promptText)
        {
            SingleLine = singleLine;
        }

        public string QuestionText { get; set; }
        public bool SingleLine { get; set; }
        public string QuestionSuffix { get; set; }

        public string ResultName { get { return QuestionName + "R"; } }

        public override QuestionResult GetResult(IValueProvider provider)
        {
            string value;
            return (TryGetValue(provider, ResultName, out value) && !string.IsNullOrEmpty(value))
                ? new QuestionResult(this) { Answer = value }
                : null;
        }
    }
}