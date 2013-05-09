using SurveyTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Linq;

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

        public SurveyQuestionDef MapQuestionDef(question_def question)
        {
            switch (question.QuestionFormat.code)
            {
                case "Header":
                    return new HeaderQuestionDef(question.question_def_id, question.prompt_text);
                case "Date":
                    return new DateQuestionDef(question.question_def_id, question.prompt_text);
                case "MultiChoice":
                    return MapMultiChoiceQuestionDef(question);
                case "YesNo":
                    return new YesNoSurveyQuestion(question.question_def_id, question.prompt_text);
                case "Text":
                    return MapTextQuestionDef(question);
                case "Int":
                    return MapIntQuestionDef(question);
                case "MultiSelect":
                    return MapMultiSelectQuestionDef(question);
                case "BloodPressure":
                    return new BloodPressureQuestionDef(question.question_def_id);
                default:
                    return null;
            }

        }

        private SurveyQuestionDef MapMultiChoiceQuestionDef(question_def questionDef)
        {
            var doc = XDocument.Parse(questionDef.question_details);
            var questions = doc.Descendants("opt").Select(x => x.Value).ToList();

            return new MultiChoiceQuestionDef(questionDef.question_def_id, questionDef.prompt_text, questions);
        }

        private SurveyQuestionDef MapIntQuestionDef(question_def questionDef)
        {
            var doc = XDocument.Parse(questionDef.question_details);

            var el = doc.Element("qt");
            var questionText = el != null ? el.Value : null;

            el = doc.Element("qs");
            var questionSuffix = el != null ? el.Value : null;

            el = doc.Element("sl");
            bool sl;
            if ((el == null) || !bool.TryParse(el.Value, out sl))
                sl = true;

            return new IntQuestionDef(questionDef.question_def_id, questionDef.prompt_text, sl)
            {
                QuestionText = questionText,
                QuestionSuffix = questionSuffix
            };
        }

        private SurveyQuestionDef MapTextQuestionDef(question_def questionDef)
        {
            var doc = XDocument.Parse(questionDef.question_details);

            var el = doc.Element("qt");
            var questionText = el != null ? el.Value : null;

            el = doc.Element("qs");
            var questionSuffix = el != null ? el.Value : null;

            el = doc.Element("sl");
            bool sl;
            if ((el == null) || !bool.TryParse(el.Value, out sl))
                sl = true;

            return new TextQuestionDef(questionDef.question_def_id, questionDef.prompt_text, sl)
            {
                QuestionText = questionText,
                QuestionSuffix = questionSuffix
            };
        }
        
        private SurveyQuestionDef MapMultiSelectQuestionDef(question_def question)
        {
            var doc = XDocument.Parse(question.question_details);
            var questions = doc.Descendants("opt").Select(x => x.Value).ToList();

            return new MultiSelectQuestionDef(question.question_def_id, question.prompt_text, questions);
        }
    }
}