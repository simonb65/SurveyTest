using SurveyTest.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SurveyTest.Models
{
    public class SurveyRepository : SurveyTest.Models.ISurveyRepository
    {
        private readonly ISurveyMapper _surveyMapper;

        public SurveyRepository(ISurveyMapper surveyMapper)
        {
            _surveyMapper = surveyMapper;
        }

        public IList<SurveyModel> ListSurveys() 
        {
            using (var db = new Repository.SurveyTestEntities())
            {
                return db.surveys.ToList().Select(s => _surveyMapper.MapSurvey(s)).ToList();
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
                    survey = survey,
                    person_name = submit.Name,
                    email_address = submit.Email,
                    date_taken = DateTime.Now
                };

                db.survey_response.Add(surveyReponse);

                foreach (var sq in surveyModel.Questions.Where(q => q.QuestionDef.HasResult))
                {
                    var sa = new survey_answer
                    {
                        survey_response = surveyReponse,
                        survey_question = survey.survey_question.First(x => x.question_def_id == sq.QuestionDef.Id),
                        survey_answer1 = sq.Answer.ToString()
                    };

                    db.survey_answer.Add(sa);
                }

                db.SaveChanges();
            }
        }
    }
}