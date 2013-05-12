using System;
using System.Web.Mvc;

namespace SurveyTest.Models
{
    public class DateQuestionDef : QuestionDef
    {
        public string DayName { get { return QuestionName + "_Day"; } }
        public string MonthName { get { return QuestionName + "_Month"; } }
        public string YearName { get { return QuestionName + "_Year"; } }

        public override string FormatType { get { return "Date"; } }

        public override QuestionResult GetResult(IValueProvider provider)
        {
            int dayVal;
            if (!TryGetValue(provider, DayName, out dayVal) || (dayVal == 0))
                return null;

            int monthVal;
            if (!TryGetValue(provider, MonthName, out monthVal) || (monthVal == 0))
                return null;

            int yearVal;
            if (!TryGetValue(provider, YearName, out yearVal) || (yearVal == 0))
                return null;

            var answer = new DateTime(yearVal, monthVal, dayVal).ToShortDateString();

            return new QuestionResult(this) { Answer = answer };
        }
    }
}