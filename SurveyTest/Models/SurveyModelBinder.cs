using System;
using System.Linq;
using System.Web.Mvc;
using System.Diagnostics;

namespace SurveyTest.Models
{
    public class SurveyModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var survey = bindingContext.Model as SurveyModel;
            if (survey == null)
                throw new ApplicationException("Need model to be passed in!");

            foreach (var q in survey.Questions.Where(q => q.QuestionDef.HasResult))
            {
                q.Answer = q.QuestionDef.GetResult(bindingContext.ValueProvider);
                if ((q.Answer == null) && (q.Mandatory))
                    bindingContext.ModelState[q.QuestionDef.QuestionName] = CreateErrorModelState(q.QuestionDef.QuestionName, "Missing");
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