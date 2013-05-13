using System;
using System.Linq;
using System.Xml.Linq;
using System.Web.Mvc;

using SurveyTest.Models;

namespace SurveyTest.Repository
{
    public interface ISurveyMapper
    {
        SurveyModel MapSurvey(Repository.survey survey);
        SurveyModel MapSurveyAndQuestions(Repository.survey survey);

        QuestionDef MapQuestionDef(question_def question);
    }

    public class SurveyMapper : ISurveyMapper
    {
        private readonly IQuestionDefActivator _qda;
        public SurveyMapper(IQuestionDefActivator qda)
        {
            _qda = qda;
        }

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
                Questions = tableOnly ? null : survey.SurveyQuestions.Select(MapSurveyQuestion).ToList()
            };
        }

        public SurveyQuestion MapSurveyQuestion(Repository.survey_question surveyQuestion)
        {
            return new SurveyQuestion
            {
                Order = surveyQuestion.question_order,
                Mandatory = surveyQuestion.mandatory ?? false,
                QuestionDef = MapQuestionDef(surveyQuestion.QuestionDef)
            };
        }

        public QuestionDef MapQuestionDef(question_def question)
        {
            QuestionDef qd = _qda.CreateQuestionDef(question.QuestionFormat.code);

            qd.Id = question.question_def_id;
            qd.Name = question.question_def_name;
            qd.PromptText = question.prompt_text;
            qd.DeserialiseDetails(question.question_details);

            return qd;
        }
       
        private QuestionDef MapMultiSelectQuestionDef(question_def question)
        {
            var doc = XDocument.Parse(question.question_details);
            var questions = doc.Descendants("opt").Select(x => x.Value).ToList();

            return new MultiSelectQuestionDef
            {
                Id = question.question_def_id,
                PromptText = question.prompt_text
                // QuestionTexts = questions
            };
        }
    }
}