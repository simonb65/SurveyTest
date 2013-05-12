using System;
using System.Web.Mvc;

using SurveyTest.Models;

namespace SurveyTest.Areas.Admin.Models
{
    public class QuestionDefModelBinder : IModelBinder
    {
        private readonly IQuestionDefActivator _qda;
        public QuestionDefModelBinder(IQuestionDefActivator qda)
        {
            _qda = qda;
        }

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            // Get the Format Type
            var formatType = GetFormatType(bindingContext);
            var qd = _qda.CreateQuestionDef(formatType);

            qd.BindFields(bindingContext);

            return qd;
        }

        private string GetFormatType(ModelBindingContext bindingContext)
        {
            var formatTypeVal = bindingContext.ValueProvider.GetValue("FormatType");
            if (formatTypeVal == null)
                throw new ApplicationException("Doh! - No format type set!");

            return formatTypeVal.AttemptedValue;
        }
    }
}