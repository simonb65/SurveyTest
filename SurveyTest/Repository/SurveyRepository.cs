using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

using SurveyTest.Models;

namespace SurveyTest.Repository
{
    public class SurveyRepository : ISurveyRepository
    {
        private readonly ISurveyMapper _surveyMapper;

        public SurveyRepository(ISurveyMapper surveyMapper)
        {
            _surveyMapper = surveyMapper;
        }

        public IList<Tuple<int, string>> ListQuestionFormats()
        {
            using (var db = new Repository.SurveyTestEntities())
            {
                return db.question_format.ToList()
                    .Select(qf => Tuple.Create(qf.question_format_id, qf.question_format1)).ToList();
            }
        }

        public IList<QuestionDef> ListQuestions()
        {
            using (var db = new Repository.SurveyTestEntities())
            {
                var r = db
                    .question_def.Include(q => q.QuestionFormat).ToList()
                    .Select(s => _surveyMapper.MapQuestionDef(s));

                return r.ToList();
            }
        }

        public QuestionDef GetQuestion(int id)
        {
            using (var db = new Repository.SurveyTestEntities())
            {
                var q = db.question_def.Find(id);
                return (q != null) ? _surveyMapper.MapQuestionDef(q) : null;
            }
        }
        
        public IList<SurveyModel> ListSurveys() 
        {
            using (var db = new Repository.SurveyTestEntities())
            {
                return db.surveys.ToList().Select(s => _surveyMapper.MapSurvey(s)).ToList();
            }
        }

        public void SaveNewQuestionDef(string name, int formatTypeId, string prompt)
        {
            using (var db = new Repository.SurveyTestEntities())
            {
                var qd = new question_def
                {
                    question_def_name = name,
                    question_format_id = formatTypeId,
                    prompt_text = prompt
                };

                db.question_def.Add(qd);
                db.SaveChanges();
            }
        }

        public void UpdateQuestionDef(QuestionDef questionDef)
        {
            using (var db = new Repository.SurveyTestEntities())
            {
                var qd = db.question_def.Find(questionDef.Id);

                qd.question_def_name = questionDef.Name;
                qd.prompt_text = questionDef.PromptText;
                qd.question_def_description = questionDef.Description;

                qd.question_details = questionDef.SerialiseDetails();

                db.SaveChanges();
            }
        }

        public SurveyModel GetSurvey(int surveyId)
        {
            using (var db = new Repository.SurveyTestEntities())
            {
                var surveys = db.surveys.Where(s => s.survey_id == surveyId);

                return surveys.Any()
                    ? _surveyMapper.MapSurveyAndQuestions(surveys.First())
                    : null;
            }
        }

        public void StoreSurveyResult(SubmitSurveyModel submit, SurveyModel surveyModel)
        {
            using (var db = new Repository.SurveyTestEntities())
            {
                var survey = db.surveys.Find(surveyModel.Id);

                var surveyReponse = new survey_response
                {
                    Survey = survey,
                    person_name = submit.Name,
                    email_address = submit.Email,
                    date_taken = DateTime.Now
                };

                db.survey_response.Add(surveyReponse);

                foreach (var sq in surveyModel.Questions.Where(q => q.QuestionDef.HasResult))
                {
                    var sa = new survey_answer
                    {
                        SurveyResponse = surveyReponse,
                        SurveyQuestion = survey.SurveyQuestions.First(x => x.question_def_id == sq.QuestionDef.Id),
                        answer = sq.Answer.ToString(),
                        
                    };

                    db.survey_answer.Add(sa);
                }

                db.SaveChanges();
            }
        }

        public void DeleteQuestion(int id)
        {
            using (var db = new Repository.SurveyTestEntities())
            {
                question_def question_def = db.question_def.Find(id);
                db.question_def.Remove(question_def);
                db.SaveChanges();
            }
        }
    }
}