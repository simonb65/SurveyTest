using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SurveyTest.Models
{
    public class MultiSelectQuestionDef : MultiChoiceQuestionDef
    {
        public override string FormatType { get { return "MultiSelect"; } }
        
        private bool QuestionSelected(IValueProvider provider, int idx)
        {
            var valResult = provider.GetValue(ResultName(idx));
            return ((valResult != null)
                && !string.IsNullOrEmpty(valResult.AttemptedValue)
                && valResult.AttemptedValue.Contains("true"));
        }

        public override QuestionResult GetResult(IValueProvider provider)
        {
            var results = new List<int>();
            var resultsValue = 0;
            for (var idx = 0; idx < QuestionOpts.Count; idx++)
            {
                if (QuestionSelected(provider, idx))
                {
                    results.Add(idx);
                    resultsValue += QuestionOpts[idx].Value;
                }
            }

            var answer = string.Join(",", results);

            return new QuestionResult { Answer = answer, Value = resultsValue };
        }
    }
}