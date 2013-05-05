using SurveyTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SurveyTest.Repository
{
    public interface ISurveyMapper
    {
        SurveyModel MapSurvey(Repository.survey survey);
        SurveyModel MapSurveyAndQuestions(Repository.survey survey);
   }

    public class SurveyMapper : ISurveyMapper
    {
        public SurveyModel MapSurvey(Repository.survey survey)
        {
            return MapSurvey(survey, true);
        }

        public SurveyModel MapSurveyAndQuestions(Repository.survey survey)
        {
            return MapSurvey(survey, false);
        }

        private SurveyModel MapSurvey(Repository.survey survey, bool tableOnly)
        {
            return new SurveyModel
            {
                Id = survey.survey_id,
                Name = survey.survey_name,
                Description = survey.survey_desc,
                Questions = tableOnly ? null : survey.survey_question.Select(MapSurveyQuestion).ToList()
            };
        }

        public SurveyQuestion MapSurveyQuestion(Repository.survey_question surveyQuestion)
        {
            return new SurveyQuestion
            {
                Order = surveyQuestion.question_order,
                Mandatory = surveyQuestion.mandatory ?? false,
                QuestionDef = MapQuestionDef(surveyQuestion.question)
            };
        }

        public SurveyQuestionDef MapQuestionDef(Repository.question question)
        {
            return null;
        }
    }
}