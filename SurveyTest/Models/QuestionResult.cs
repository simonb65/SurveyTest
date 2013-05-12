using System;

namespace SurveyTest.Models
{
    public class QuestionResult
    {
        public QuestionDef Question { get; private set; }
        public QuestionResult(QuestionDef question)
        {
            Question = question;
        }

        public int Value { get; set; }
        public string Answer { get; set; }

        public override string ToString() { return Answer; }
    }
}