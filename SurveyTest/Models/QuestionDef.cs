using System;
using System.Web.Mvc;

namespace SurveyTest.Models
{
    public abstract class QuestionDef
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PromptText { get; set; }
        public string Description { get; set; }

        public abstract string FormatType { get; }

        public virtual string QuestionName { get { return "Q" + Id; } }
        public virtual bool HasResult { get { return true; } }

        public virtual void MapFields(Repository.question_def question)
        {
            Id = question.question_def_id;
            Name = question.question_def_name;
            PromptText = question.prompt_text;

            DeserialiseDetails(question.question_details);
        }

        public virtual void BindFields(ModelBindingContext bindingContext)
        {
            int tmpInt;
            string tmpStr;

            // Base Field values
            if (TryGetValue(bindingContext.ValueProvider, "Id", out tmpInt))
                Id = tmpInt;
            else
                bindingContext.ModelState["Id"] = new ModelState { Errors = { "Id not Defined " } };

            if (TryGetValue(bindingContext.ValueProvider, "Name", out tmpStr) && !string.IsNullOrEmpty(tmpStr))
                Name = tmpStr;
            else
                bindingContext.ModelState["Name"] = new ModelState { Errors = { "Name is mandatory" } };

            if (TryGetValue(bindingContext.ValueProvider, "PromptText", out tmpStr) && !string.IsNullOrEmpty(tmpStr))
                PromptText = tmpStr;
            else
                bindingContext.ModelState["PromptText"] = new ModelState { Errors = { "PromptText is mandatory" } };
        }

        public virtual string SerialiseDetails()
        {
            return null;
        }

        public virtual void DeserialiseDetails(string details)
        {
        }

        public abstract QuestionResult GetResult(IValueProvider provider);

        // Utility Methods
        protected static bool TryGetValue(IValueProvider provider, string key, out int value)
        {
            value = 0;
            var valueResult = provider.GetValue(key);
            return ((valueResult != null)
                && !string.IsNullOrEmpty(valueResult.AttemptedValue)
                && int.TryParse(valueResult.AttemptedValue, out value));
        }

        protected static bool TryGetValue(IValueProvider provider, string key, out string value)
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