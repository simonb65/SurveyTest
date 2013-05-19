using System;

namespace SurveyTest.Models
{
    public class SurveyQuestionResult
    {
        public SurveyQuestion SurveyQuestion { get; private set; }
        public SurveyQuestionResult(SurveyQuestion surveyQuestion)
        {
            SurveyQuestion = surveyQuestion;
        }

        public int Value { get; set; }
        public string Answer { get; set; }

        public override string ToString() { return Answer; }
    }
}