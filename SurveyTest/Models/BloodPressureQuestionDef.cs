using System;
using System.Web.Mvc;

namespace SurveyTest.Models
{
    public class BloodPressureQuestionDef : QuestionDef
    {
        public string SysName { get { return QuestionName + "Sys"; } }
        public string DiaName { get { return QuestionName + "Dia"; } }

        public override string FormatType { get { return "BloodPressure"; } }

        public override QuestionResult GetResult(IValueProvider provider)
        {
            int sysPress;
            int diaPress;

            if (!TryGetValue(provider, SysName, out sysPress))
                return null;

            if (!TryGetValue(provider, DiaName, out diaPress))
                return null;

            var answer = sysPress + "/" + diaPress;

            return new QuestionResult { Answer = answer, Value = 0};
        }
    }
}