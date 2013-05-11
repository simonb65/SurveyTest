using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SurveyTest.Models
{
    public class MultiSelectQuestionDef : MultiChoiceQuestionDef
    {
        public MultiSelectQuestionDef(int id, string promptText, IList<string> questionTexts)
            : base(id, promptText, questionTexts)
        {
        }

        private bool QuestionSelected(IValueProvider provider, int idx)
        {
            var valResult = provider.GetValue(ResultName(idx));
            return ((valResult != null)
                && !string.IsNullOrEmpty(valResult.AttemptedValue)
                && valResult.AttemptedValue.Contains("true"));
        }

        public override QuestionResult GetResult(IValueProvider provider)
        {
            var results = new List<string>();
            for (var idx = 0; idx < QuestionTexts.Count; idx++)
            {
                if (QuestionSelected(provider, idx))
                    results.Add(QuestionTexts[idx]);
            }

            var answer = string.Join("|", results);

            return (results.Count > 0) ? new QuestionResult(this) { Answer = answer } : null;
        }
    }
}