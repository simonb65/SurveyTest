using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SurveyTest.Models
{
    public interface IQuestionDefActivator
    {
        QuestionDef CreateQuestionDef(string formatType);
    }

    public class QuestionDefActivator : IQuestionDefActivator
    {
        private static readonly IDictionary<string, Func<QuestionDef>> _creatorMap = new Dictionary<string, Func<QuestionDef>>(StringComparer.OrdinalIgnoreCase)
        {
            { "Header", () => new HeaderQuestionDef() },
            { "Date", () => new DateQuestionDef() },
            { "MultiChoice", () => new MultiChoiceQuestionDef() },
            { "YesNo", () => new YesNoSurveyQuestionDef() }, 
            { "Text", () => new TextQuestionDef() },
            { "Int", () => new IntQuestionDef() },
            { "MultiSelect", () => new  MultiSelectQuestionDef() },
            { "BloodPressure", () => new BloodPressureQuestionDef() }
        };

        public QuestionDef CreateQuestionDef(string formatType)
        {
            Func<QuestionDef> creator;

            if (!_creatorMap.TryGetValue(formatType, out creator))
                throw new ApplicationException("Unknown Question Format - " + formatType);

            return creator();
        }
    }
}