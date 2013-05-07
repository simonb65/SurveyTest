using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SurveyTest.Models
{
    public abstract class QuestionResult
    {
        public SurveyQuestionDef Question { get; private set; }            
        public QuestionResult (SurveyQuestionDef question)
        {
                Question = question;
        }
    }

    public class SimpleQuestionResult<T> : QuestionResult
    {
        public SimpleQuestionResult(SurveyQuestionDef question)
            : base(question)
        {
        }

        public T Value { get; set; }
        public override string ToString() { return Value.ToString(); } 
    }

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

    public class HeaderQuestionDef : SurveyQuestionDef
    {
        public HeaderQuestionDef(int id, string promptText)
            : base(id, promptText)
        {
        }

        public override bool HasResult { get  {  return false; } }
        public override QuestionResult GetResult(IValueProvider provider) { return null; }
    }

    public class DateQuestionDef : SurveyQuestionDef
    {
        public DateQuestionDef(int id, string promptText)
            : base(id, promptText)
        {
        }

        public string DayName { get { return QuestionName + "_Day"; } }
        public string MonthName { get { return QuestionName + "_Month"; } }
        public string YearName { get { return QuestionName + "_Year"; } }

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

            return new SimpleQuestionResult<DateTime>(this) { Value = new DateTime(yearVal, monthVal, dayVal) };
        }
    }

    public class MultiChoiceQuestionResult : SimpleQuestionResult<int>
    {
        public MultiChoiceQuestionResult(MultiChoiceQuestionDef question)
            : base(question)
        {

        }
        public override string ToString()
        {
            return ((MultiChoiceQuestionDef)Question).QuestionTexts[Value];
        }
    }

    public class MultiChoiceQuestionDef : SurveyQuestionDef
    {
        public IList<string> QuestionTexts { get; protected set; }
        public int DefaultIdx { get; protected set; }

        public MultiChoiceQuestionDef(int id, string promptText, IList<string> questionTexts, int defaultIdx = -1)
            : base(id, promptText)
        {
            QuestionTexts = questionTexts;
            DefaultIdx = defaultIdx;
        }

        public string GroupName { get { return QuestionName + "_G"; } }

        protected string ResultPrefix { get { return QuestionName + "_R";  } }
        public string ResultName(int idx) { return ResultPrefix + idx.ToString("00"); }

        public override QuestionResult GetResult(IValueProvider provider)
        {
            string value;
            if (TryGetValue(provider, GroupName, out value))
            {
                var idx = int.Parse(value.Substring(ResultPrefix.Length));
                return new MultiChoiceQuestionResult(this) { Value = idx };
            }
            
            return null;
        }
    }

    public class YesNoSurveyQuestion : MultiChoiceQuestionDef
    {
        public YesNoSurveyQuestion(int id, string promptText)
            : base(id, promptText, new [] { "Yes", "No" } )
        {
        }
        public override string FormatType { get { return GetType().BaseType.Name; } }

        public override QuestionResult GetResult(IValueProvider provider)
        {
            string value;
            if (TryGetValue(provider, GroupName, out value))
            {
                var idx = int.Parse(value.Substring(ResultPrefix.Length));
                return new SimpleQuestionResult<bool>(this) { Value = (idx == 0) };
            }

            return null;
        }
    }

    public class TextQuestionDef : SurveyQuestionDef
    {
        public TextQuestionDef(int id, string promptText, bool singleLine = true)
            : base(id, promptText)
        {
            SingleLine = singleLine;
        }

        public string QuestionText { get; set; }
        public bool SingleLine { get; set; }
        public string QuestionSuffix { get; set; }

        public string ResultName { get { return QuestionName + "R"; } }

        public override QuestionResult GetResult(IValueProvider provider)
        {
            string value;
            return (TryGetValue(provider, ResultName, out value) && !string.IsNullOrEmpty(value))
                ? new SimpleQuestionResult<string>(this) { Value = value }
                : null;
        }
    }

    public class IntQuestionDef : TextQuestionDef
    {
        public IntQuestionDef(int id, string promptText, bool singleLine = true)
            : base(id, promptText, singleLine)
        {
        }

        public override string FormatType { get { return GetType().BaseType.Name; } }

        public override QuestionResult GetResult(IValueProvider provider)
        {
            int value;
            return (TryGetValue(provider, ResultName, out value))
                ? new SimpleQuestionResult<int>(this) { Value = value }
                : null;
        }
    }

    public class MultiSelectQuestionResult : SimpleQuestionResult<IList<int>>
    {
        public MultiSelectQuestionResult(MultiSelectQuestionDef question)
            : base(question)
        {

        }
        public override string ToString()
        {
            var qts = ((MultiSelectQuestionDef)Question).QuestionTexts;
            return string.Join("|", Value.Select(x => qts[x]));
        }
    }

    public class MultiSelectQuestionDef : MultiChoiceQuestionDef
    {
        public MultiSelectQuestionDef(int id, string promptText, IList<string> questionTexts)
            : base(id, promptText, questionTexts)
        {
        }

        private bool QuestionSelected(IValueProvider provider, int idx)
        {
            var valResult = provider.GetValue(ResultName(idx));
            return ((valResult != null) 
                && !string.IsNullOrEmpty(valResult.AttemptedValue) 
                && valResult.AttemptedValue.Contains("true"));
        }

        public override QuestionResult GetResult(IValueProvider provider)
        {
            var results = new List<int>();
            for (var idx = 0; idx < QuestionTexts.Count; idx++)
            {
                if (QuestionSelected(provider, idx))
                    results.Add(idx);
            }

            return (results.Count > 0) ? new MultiSelectQuestionResult(this) { Value = results } : null;
        }
    }

    public class BloodPressureQuestionResult : QuestionResult
    {
        public BloodPressureQuestionResult(SurveyQuestionDef question, int sysPress, int diaPress)
            : base(question)
        {
            SysPress = sysPress;
            DiaPress = diaPress;
        }
        
        public int SysPress { get; private set; }
        public int DiaPress { get; private set; }

        public override string ToString() { return SysPress + "/" + DiaPress; } 

    }

    public class BloodPressureQuestionDef : SurveyQuestionDef
    {
        public BloodPressureQuestionDef(int id)
            : base(id, "Blood Pressure")
        {
        }

        public string SysName { get { return QuestionName + "Sys"; } }
        public string DiaName { get { return QuestionName + "Dia"; } }

        public override QuestionResult GetResult(IValueProvider provider)
        {
            int sysPress;
            int diaPress;

            if (!TryGetValue(provider, SysName, out sysPress))
                return null;

            if (!TryGetValue(provider, DiaName, out diaPress))
                return null;

            return new BloodPressureQuestionResult(this, sysPress, diaPress);
        }
    }

    public class SurveyQuestion
    {
        public SurveyQuestionDef QuestionDef { get; set; }
        public int Order { get; set; }
        public bool Mandatory { get; set; }
        public QuestionResult Answer { get; set; }
    }

    public class SurveyModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IList<SurveyQuestion> Questions { get; set; }
    }
}