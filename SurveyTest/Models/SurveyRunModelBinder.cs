using System;
using System.Linq;
using System.Web.Mvc;
using System.Diagnostics;
using System.Collections.Generic;

namespace SurveyTest.Models
{
    public class SurveyRunModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var survey = bindingContext.Model as SurveyRunModel;
            if (survey == null)
                throw new ApplicationException("Need model to be passed in!");

            foreach (var q in survey.Questions.Where(q => q.QuestionDef.HasResult))
            {
                var answer = q.QuestionDef.GetResult(bindingContext.ValueProvider);
                if ((answer == null) && (q.Mandatory))
                    bindingContext.ModelState[q.QuestionDef.QuestionName] = CreateErrorModelState(q.QuestionDef.QuestionName, "Missing");

                q.Answer = answer;
            }

            return survey;
        }

        private static ModelState CreateErrorModelState(string field, string message)
        {
            var ms = new ModelState();
            ms.Errors.Add(message);

            return ms;
        }
    }
}