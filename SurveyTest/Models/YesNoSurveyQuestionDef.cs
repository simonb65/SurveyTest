using System;
using System.Linq;
using System.Web.Mvc;
using System.Xml.Linq;

namespace SurveyTest.Models
{
    public class YesNoSurveyQuestionDef : QuestionDef
    {
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

        public string GroupName { get { return QuestionName + "_G"; } }
        protected string ResultPrefix { get { return QuestionName + "_R"; } }

        public override void BindFields(ModelBindingContext bindingContext)
        {
            base.BindFields(bindingContext);

            int tmpInt;

            if (TryGetValue(bindingContext.ValueProvider, "YesOptionValue", out tmpInt))
                YesOptionValue = tmpInt;

            if (TryGetValue(bindingContext.ValueProvider, "NoOptionValue", out tmpInt))
                NoOptionValue = tmpInt;
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
            YesOptionValue = el != null ? int.Parse(el.Attribute("value").Value) : 0;

            el = doc.Descendants("no").FirstOrDefault();
            NoOptionValue = el != null ? int.Parse(el.Attribute("value").Value) : 0;
        }
    }
}