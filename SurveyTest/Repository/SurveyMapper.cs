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
            switch (question.question_format.code)
            {
                case "Header":
                    return new HeaderQuestionDef(question.question_id, question.prompt_text);
                case "Date":
                    return new DateQuestionDef(question.question_id, question.prompt_text);
                case "MultiChoice":
                    return MapMultiChoiceQuestionDef(question);
                case "YesNo":
                    return new YesNoSurveyQuestion(question.question_id, question.prompt_text);
                case "Text":
                    return MapTextQuestionDef(question);
                case "Int":
                    return MapIntQuestionDef(question);
                case "MultiSelect":
                    return MapMultiSelectQuestionDef(question);
                case "BloodPressure":
                    return new BloodPressureQuestionDef(question.question_id);
                default:
                    return null;
            }

        }

        private SurveyQuestionDef MapMultiChoiceQuestionDef(question question)
        {
            var doc = XDocument.Parse(question.question_details);
            var questions = doc.Descendants("opt").Select(x => x.Value).ToList();

            return new MultiChoiceQuestionDef(question.question_id, question.prompt_text, questions);
        }

        private SurveyQuestionDef MapIntQuestionDef(question question)
        {
            var doc = XDocument.Parse(question.question_details);

            var el = doc.Element("qt");
            var questionText = el != null ? el.Value : null;

            el = doc.Element("qs");
            var questionSuffix = el != null ? el.Value : null;

            el = doc.Element("sl");
            bool sl;
            if ((el == null) || !bool.TryParse(el.Value, out sl))
                sl = true;

            return new IntQuestionDef(question.question_id, question.prompt_text, sl)
            {
                QuestionText = questionText,
                QuestionSuffix = questionSuffix
            };
        }

        private SurveyQuestionDef MapTextQuestionDef(question question)
        {
            var doc = XDocument.Parse(question.question_details);

            var el = doc.Element("qt");
            var questionText = el != null ? el.Value : null;

            el = doc.Element("qs");
            var questionSuffix = el != null ? el.Value : null;

            el = doc.Element("sl");
            bool sl;
            if ((el == null) || !bool.TryParse(el.Value, out sl))
                sl = true;

            return new TextQuestionDef(question.question_id, question.prompt_text, sl)
            {
                QuestionText = questionText,
                QuestionSuffix = questionSuffix
            };
        }
        
        private SurveyQuestionDef MapMultiSelectQuestionDef(question question)
        {
            var doc = XDocument.Parse(question.question_details);
            var questions = doc.Descendants("opt").Select(x => x.Value).ToList();

            return new MultiSelectQuestionDef(question.question_id, question.prompt_text, questions);
        }
    }
}