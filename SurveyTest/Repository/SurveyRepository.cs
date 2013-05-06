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
    }
}