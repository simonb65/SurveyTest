using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace SurveyTest.Models
{
    public class MultiChoiceQuestionDef : QuestionDef
    {
        public IList<string> QuestionTexts { get; set; }
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
                return new QuestionResult(this) { Answer = QuestionTexts[idx] };
            }

            return null;
        }
    }
}