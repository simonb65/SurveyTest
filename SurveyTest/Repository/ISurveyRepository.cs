using System;
using System.Collections.Generic;

namespace SurveyTest.Models
{
    public interface ISurveyRepository
    {
        IList<SurveyModel> ListSurveys();
        SurveyModel GetSurvey(int surveyId);

        void StoreSurveyResult(SubmitSurveyModel submit, SurveyModel survey);
    }
}
