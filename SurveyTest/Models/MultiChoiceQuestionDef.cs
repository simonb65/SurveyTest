using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Xml.Linq;

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
                    Questions.Select(t => string.Format("<opt value=\"{0}\" order=\"{1}\">{2}</opt>", t.Value, t.Order, t.Text))) + 
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
                    .Select(x =>
                        new QuestionOption
                        {
                            Text = x.Value,
                            Value = int.Parse(x.Attribute("value").Value),
                            Order = int.Parse(x.Attribute("order").Value)
                        })
                    .OrderBy(x => x.Order)
                    .ToList();
            }
        }
    }
}