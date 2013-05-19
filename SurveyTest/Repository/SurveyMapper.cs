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
        void MapSurveyAndQuestions(Repository.survey surveyRec, SurveyModel surveyModel);

        QuestionDef MapQuestionDef(question_def question);
    }

    public class SurveyMapper : ISurveyMapper
    {
        private readonly IQuestionDefActivator _qda;
        public SurveyMapper(IQuestionDefActivator qda)
        {
            _qda = qda;
        }

        public SurveyModel MapSurvey(Repository.survey surveyRec)
        {
            var surveyModel = new SurveyModel();
            MapSurvey(surveyRec, surveyModel, false);
            return surveyModel;
        }

        public void MapSurveyAndQuestions(Repository.survey surveyRec, SurveyModel surveyModel)
        {
            MapSurvey(surveyRec, surveyModel, true);
        }

        private void MapSurvey(Repository.survey surveyRec, SurveyModel surveyModel, bool mapQuestions)
        {
            surveyModel.Id = surveyRec.survey_id;
            surveyModel.Name = surveyRec.survey_name;
            surveyModel.Description = surveyRec.survey_desc;
            surveyModel.Questions = mapQuestions
                    ? surveyRec.SurveyQuestions.Select(MapSurveyQuestion).OrderBy(x => x.Order).ToList()
                    : null ;
        }

        public SurveyQuestion MapSurveyQuestion(Repository.survey_question surveyQuestion)
        {
            return new SurveyQuestion
            {
                Id = surveyQuestion.survey_question_id,
                Order = surveyQuestion.question_order,
                Mandatory = surveyQuestion.mandatory ?? false,
                Question = MapQuestionDef(surveyQuestion.QuestionDef)
            };
        }

        public QuestionDef MapQuestionDef(question_def question)
        {
            QuestionDef qd = _qda.CreateQuestionDef(question.QuestionFormat.code);

            qd.Id = question.question_def_id;
            qd.Name = question.question_def_name;
            qd.PromptText = question.prompt_text;
            qd.Description = question.question_def_description;
            qd.DeserialiseDetails(question.question_details);

            return qd;
        }
    }
}