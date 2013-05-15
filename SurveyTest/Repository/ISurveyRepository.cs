using System;
using System.Collections.Generic;

using SurveyTest.Models;

namespace SurveyTest.Repository
{
    public interface ISurveyRepository
    {
        IList<SurveyModel> ListSurveys();
        SurveyModel GetSurvey(int surveyId);
        void SaveSurvey(SurveyModel survey);

        void StoreSurveyResult(SubmitSurveyModel submit, SurveyModel survey);

        IList<QuestionDef> ListQuestions();
        IList<Tuple<int, string>> ListQuestionFormats();

        void SaveNewQuestionDef(string name, int formatTypeId, string prompt);
        void UpdateQuestionDef(QuestionDef questionDef);

        QuestionDef GetQuestion(int id);

        void DeleteQuestion(int id);
    }
}
