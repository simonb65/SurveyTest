using System;
using System.Web.Mvc;

namespace SurveyTest.Models
{
    public class YesNoSurveyQuestionDef : MultiChoiceQuestionDef
    {
        public YesNoSurveyQuestionDef()
        {
            Questions = new[]
            { 
                new QuestionOption { Text= "Yes" },
                new QuestionOption { Text= "No" },
            };
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

        public int YesOptionValue { get; set; }
        public int NoOptionValue { get; set; }

        public override void BindFields(ModelBindingContext bindingContext)
        {
            base.BindFields(bindingContext);

            int tmpInt;

            if (TryGetValue(bindingContext.ValueProvider, "YesOptionValue", out tmpInt))
                YesOptionValue = tmpInt;

            if (TryGetValue(bindingContext.ValueProvider, "NoOptionValue", out tmpInt))
                NoOptionValue = tmpInt;
        }
    }
}