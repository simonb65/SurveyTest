using System;
using System.Linq;
using System.Web.Mvc;
using System.Xml.Linq;

namespace SurveyTest.Models
{
    public class YesNoQuestionDef : MultiChoiceQuestionDef
    {
        public YesNoQuestionDef()
        {
            QuestionOpts = new[]    // Order is explicit here as it's referenced
            {
                new QuestionOption { Text = "Yes", Order = 1, Value = 0 },
                new QuestionOption { Text = "No", Order = 2, Value = 0 }
            };
        }

        public int YesOptionValue
        {
            get { return QuestionOpts[0].Value; }
            set { QuestionOpts[0].Value = value; }
        }

        public int NoOptionValue
        {
            get { return QuestionOpts[1].Value; }
            set { QuestionOpts[1].Value = value; }
        }

        public override QuestionResult GetResult(IValueProvider provider)
        {
            string value;
            if (TryGetValue(provider, GroupName, out value))
            {
                var idx = int.Parse(value.Substring(ResultPrefix.Length));
                var answer = (idx == 0);

                return new QuestionResult { Answer = answer.ToString(), Value = answer ? YesOptionValue : NoOptionValue };
            }

            return null;
        }
        
        protected override void BindOptions(ModelBindingContext bindingContext)
        {
            int optValue;
            if (TryGetValue(bindingContext.ValueProvider, "YesOptionValue", out optValue))
                YesOptionValue = optValue;

            if (TryGetValue(bindingContext.ValueProvider, "NoOptionValue", out optValue))
                NoOptionValue = optValue;
        }

        public override string SerialiseDetails()
        {
            return string.Format("<opts><yes value=\"{0}\"/><no value=\"{1}\"/></opts>", YesOptionValue, NoOptionValue);
        }

        public override void DeserialiseDetails(string details)
        {
            if (string.IsNullOrEmpty(details))
                return;

            var doc = XDocument.Parse(details);

            var el = doc.Descendants("yes").FirstOrDefault();
            QuestionOpts[0].Value = el != null ? int.Parse(el.Attribute("value").Value) : 0;

            el = doc.Descendants("no").FirstOrDefault();
            QuestionOpts[1].Value = el != null ? int.Parse(el.Attribute("value").Value) : 0;
        }
    }
}