using System;
using System.Linq;
using System.Web.Mvc;
using System.Xml.Linq;

namespace SurveyTest.Models
{
    public class TextQuestionDef : QuestionDef
    {
        public string QuestionText { get; set; }
        public bool SingleLine { get; set; }
        public string QuestionSuffix { get; set; }

        public override string FormatType { get { return "Text"; } }

        public string ResultName { get { return QuestionName + "R"; } }

        public override string GetResult(IValueProvider provider)
        {
            string value;
            return (TryGetValue(provider, ResultName, out value) && !string.IsNullOrEmpty(value))
                ? value 
                : null;
        }

        public override string SerialiseDetails()
        {
            return string.Format("<details><qt>{0}</qt><qs>{1}</qs><sl>{2}</sl></details>", QuestionText, QuestionSuffix, SingleLine);
        }

        public override void DeserialiseDetails(string details)
        {
            if (string.IsNullOrEmpty(details))
                return;

            var doc = XDocument.Parse(details);

            var el = doc.Descendants("qt").FirstOrDefault();
            QuestionText = el != null ? el.Value : null;

            el = doc.Descendants("qs").FirstOrDefault();
            QuestionSuffix = el != null ? el.Value : null;

            el = doc.Descendants("sl").FirstOrDefault();
            bool sl;
            if ((el == null) || !bool.TryParse(el.Value, out sl))
                sl = true;
            SingleLine = sl;
        }


        public override void BindFields(ModelBindingContext bindingContext)
        {
            base.BindFields(bindingContext);

            string tmp;
            if (TryGetValue(bindingContext.ValueProvider, "QuestionText", out tmp))
                QuestionText = tmp;

            if (TryGetValue(bindingContext.ValueProvider, "QuestionSuffix", out tmp))
                QuestionSuffix = tmp;

            bool sl;
            if (TryGetValue(bindingContext.ValueProvider, "SingleLine", out sl))
                SingleLine = sl;
        }
    }
}