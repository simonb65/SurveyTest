using SurveyTest.Models;
using SurveyTest.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestionLoader
{
    public class InMemorySurveyRepository // : ISurveyRepository
    {
        private static QuestionDef[] _questions = new QuestionDef[]
        {
            //new HeaderQuestionDef(0, "Personal Details"),
            //new BloodPressureQuestionDef(100),
            //new IntQuestionDef(101, "Total Cholesterol") { QuestionSuffix = "mmol/L (if known)" },
            //new IntQuestionDef(102, "Blood Glucose") { QuestionSuffix = "mmol/L (if known)" },

            //new MultiSelectQuestionDef(103, "Current medical conditions", new [] 
            //{
            //    "Heart Disease",
            //    "High cholesterol",
            //    "Type 2 diabetes",
            //    "Asthma",
            //    "Overweight or obesity",
            //    "Hayfever",
            //    "Anxiety or depression",
            //    "Back pain or other chronic pain",
            //    "High blood pressure"
            //}),

            //new DateQuestionDef(1, "1. Date of birth"),
            //new MultiChoiceQuestionDef(2, "2. Your Gender?", new [] { "Female", "Male" }),
            //new MultiChoiceQuestionDef(3, "3. Your ethnicity/country of birth?", new [] 
            //{
            //    "Aboriginal, Torres Strait Islander, Pacific Islander or Maori",
            //    "Australia",
            //    "Asia (including the Indian sub-continent), Middle East, North Africa, Southern Europe",
            //    "Other"
            //}),
            //new YesNoSurveyQuestionDef(4, "4. Have either of your parents, or any of your brothers or sisters been diagnosed with diabetes (type 1 or type 2)?"),
            //new YesNoSurveyQuestionDef(5, "5.  Have you ever been found to have high blood glucose (blood sugar) (for example, in a health examination, during an illness, during pregnancy)?"),
            //new YesNoSurveyQuestionDef(6, "6. Are you currently taking medication for high blood pressure?"),
            //new YesNoSurveyQuestionDef(7, "7. Do you currently smoke cigarettes or any other tobacco products on a daily basis?"),
            //new MultiChoiceQuestionDef(8, "8.  How often do you eat vegetables or fruit?", new [] 
            //{
            //    "Every day",
            //    "Not every day"
            //}),
            //new YesNoSurveyQuestionDef(9, "9.  On average, would you say you do at least 2.5 hours of physical activity per week (for example, 30 minutes a day on 5 or more days a week)?"),
            //new IntQuestionDef(10, "10.  Your waist measurement taken below the ribs (usually at the level of the navel, and while standing)")
            //{
            //    QuestionText = "Waist measurement (cm)",
            //    SingleLine =true
            //}
        };


        private static SurveyModel[] _surveys = new SurveyModel[]
        {
            new SurveyModel
            {
                Id = 1,
                Name = "Remedy HRA",
                Description = "Welcome to the Remedy Healthcare risk assessment tool",
                Questions = _questions
                .Select((q, idx) => new SurveyQuestion { Order = idx, Question = q, Mandatory = q.HasResult })
                .ToList()
            },
        };

        public IList<SurveyModel> ListSurveys() { return _surveys; }

        public SurveyModel GetSurvey(int surveyId)
        {
            return _surveys.FirstOrDefault(s => s.Id == surveyId);
        }

        public void StoreSurveyResult(SubmitSurveyModel submit, SurveyModel survey)
        {
            // Yes yes, jolly good wot
        }

        public IList<QuestionDef> ListQuestions()
        {
            return _questions;
        }

        public IList<Tuple<int, string>> ListQuestionFormats()
        {
            return null;
        }

        public void UpdateQuestionDef(QuestionDef questionDef)
        {

        }

        public void SaveNewQuestionDef(string name, int formatTypeId, string prompt) { }


        public QuestionDef GetQuestion(int id)
        {
            throw new NotImplementedException();
        }
    }
}
