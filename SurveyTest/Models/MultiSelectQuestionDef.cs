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
            var results = new List<string>();
            var resultsValue = 0;
            for (var idx = 0; idx < Questions.Count; idx++)
            {
                if (QuestionSelected(provider, idx))
                {
                    results.Add(Questions[idx].Text);
                    resultsValue += Questions[idx].Value;
                }
            }

            var answer = string.Join("|", results);

            return (results.Count > 0) ? new QuestionResult(this) { Answer = answer, Value = resultsValue } : null;
        }
    }
}