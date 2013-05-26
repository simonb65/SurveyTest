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

            foreach (var sq in survey.Questions.Where(q => q.Question.HasResult))
            {
                var result = sq.Question.GetResult(bindingContext.ValueProvider);
                if ((result == null) && (sq.Mandatory))
                    bindingContext.ModelState[sq.Question.QuestionName] = CreateErrorModelState(sq.Question.QuestionName, "Missing");

                if (result != null)
                {
                    survey.Answers[sq].Answer = result.Answer;
                    survey.Answers[sq].Value = result.Value;
                }
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