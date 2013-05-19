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


        public virtual string QuestionCode 
        { 
            get 
            { 
                var typeName = GetType().Name;
                return typeName.Substring(0, typeName.Length - 11);    // QuestionDef has 11 char
            }
        }
        public abstract string FormatType { get; }

        public virtual string QuestionName { get { return "Q" + Id; } }
        public virtual bool HasResult { get { return true; } }

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

            if (TryGetValue(bindingContext.ValueProvider, "Description", out tmpStr) && !string.IsNullOrEmpty(tmpStr))
                Description = tmpStr;

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

        public abstract string GetResult(IValueProvider provider);

        public override int GetHashCode() { return Id; }

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
            return false;
        }

        protected static bool TryGetValue(IValueProvider provider, string key, out bool value)
        {
            value = false;
            var valueResult = provider.GetValue(key);
            if ((valueResult != null) && !string.IsNullOrEmpty(valueResult.AttemptedValue))
            {
                value = valueResult.AttemptedValue.Contains("true");
                return true;
            }
            return false;
        }

    }
}