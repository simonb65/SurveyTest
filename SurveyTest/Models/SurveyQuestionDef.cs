using System;
using System.Web.Mvc;

namespace SurveyTest.Models
{
    public abstract class SurveyQuestionDef
    {
        public SurveyQuestionDef(int id, string promptText)
        {
            Id = id;
            PromptText = promptText;
        }

        public int Id { get; private set; }
        public virtual string FormatType { get { return GetType().Name; } }
        public string PromptText { get; protected set; }

        public virtual string QuestionName { get { return "Q" + Id; } }
        public virtual bool HasResult { get { return true; } }

        public abstract QuestionResult GetResult(IValueProvider provider);

        // Utility Methods
        protected bool TryGetValue(IValueProvider provider, string key, out int value)
        {
            value = 0;
            var valueResult = provider.GetValue(key);
            return ((valueResult != null)
                && !string.IsNullOrEmpty(valueResult.AttemptedValue)
                && int.TryParse(valueResult.AttemptedValue, out value));
        }

        protected bool TryGetValue(IValueProvider provider, string key, out string value)
        {
            value = null;
            var valueResult = provider.GetValue(key);
            if (valueResult != null)
            {
                value = valueResult.AttemptedValue;
                return true;
            }
            else
                return false;
        }
    }
}