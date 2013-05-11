using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace SurveyTest.Models
{
    public class MultiChoiceQuestionDef : SurveyQuestionDef
    {
        public IList<string> QuestionTexts { get; protected set; }
        public int DefaultIdx { get; protected set; }

        public MultiChoiceQuestionDef(int id, string promptText, IList<string> questionTexts, int defaultIdx = -1)
            : base(id, promptText)
        {
            QuestionTexts = questionTexts;
            DefaultIdx = defaultIdx;
        }

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