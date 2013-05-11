using System;

namespace SurveyTest.Models
{
    public class QuestionResult
    {
        public SurveyQuestionDef Question { get; private set; }
        public QuestionResult(SurveyQuestionDef question)
        {
            Question = question;
        }

        public int Value { get; set; }
        public string Answer { get; set; }

        public override string ToString() { return Answer; }
    }
}