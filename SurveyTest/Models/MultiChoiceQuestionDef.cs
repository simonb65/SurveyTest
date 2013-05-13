using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Xml.Linq;
using System.Globalization;

namespace SurveyTest.Models
{
    public class MultiChoiceQuestionDef : QuestionDef
    {
        public class QuestionOption
        {
            public string Text { get; set; }
            public int Order { get; set; }
            public int Value { get; set; }
        }

        public IList<QuestionOption> Questions { get; set; }
        public int DefaultIdx { get; set; }

        public MultiChoiceQuestionDef()
        {
            DefaultIdx = -1;
        }

        public override string FormatType { get { return "MultiChoice"; } }
        
        public string GroupName { get { return QuestionName + "_G"; } }

        protected string ResultPrefix { get { return QuestionName + "_R"; } }
        public string ResultName(int idx) { return ResultPrefix + idx.ToString("00"); }

        public override QuestionResult GetResult(IValueProvider provider)
        {
            string value;
            if (TryGetValue(provider, GroupName, out value))
            {
                var idx = int.Parse(value.Substring(ResultPrefix.Length));
                var answer = Questions[idx];
                return new QuestionResult(this) { Answer = answer.Text, Value = answer.Value };
            }

            return null;
        }
         
        public override string SerialiseDetails()
        {
            return "<opts>" + 
                string.Join(
                    string.Empty,
                    Questions
                        .OrderBy(x => x.Order)
                        .Select(t => string.Format("<opt value=\"{0}\">{1}</opt>", t.Value, t.Text))) + 
                "</opts>";
        }

        public override void DeserialiseDetails(string details)
        {
            if (details == null)
            {
                Questions = new List<QuestionOption>();
            }
            else
            {
                var doc = XDocument.Parse(details);
                Questions = doc.Descendants("opt")
                    .Select((x, idx) =>
                        new QuestionOption
                        {
                            Text = x.Value,
                            Value = int.Parse(x.Attribute("value").Value),
                            Order = idx
                        })
                    .OrderBy(x => x.Order)
                    .ToList();
            }
        }

        public override void BindFields(ModelBindingContext bindingContext)
        {
            base.BindFields(bindingContext);

            // Get Questions
            var questions = new List<QuestionOption>();

            for (var idx = 0; ; idx++)
            {
                var textFieldName = string.Format("Questions[{0}].Text", idx);
                string optTxt;
                if (!TryGetValue(bindingContext.ValueProvider, textFieldName, out optTxt))
                    break;

                var valueFieldName = string.Format("Questions[{0}].Value", idx);
                int optValue;
                if (!TryGetValue(bindingContext.ValueProvider, valueFieldName, out optValue))
                    optValue = 0;

                questions.Add(new QuestionOption { Text = optTxt, Value = optValue, Order = idx });
            }

            Questions = questions;
        }
    }
}
