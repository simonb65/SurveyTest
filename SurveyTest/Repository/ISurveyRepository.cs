﻿using System;
using System.Collections.Generic;

using SurveyTest.Models;

namespace SurveyTest.Repository
{
    public interface ISurveyRepository
    {
        IList<SurveyModel> ListSurveys();
        SurveyModel GetSurvey(int surveyId);
        void SaveSurvey(SurveyModel survey);
        void DeleteSurvey(int id);

        SurveyRunModel GetSurveyRunMode(int surveyId);
        void StoreSurveyResult(SubmitSurveyModel submit, SurveyRunModel survey);

        IList<Tuple<int, string>> ListQuestionFormats();

        IList<QuestionDef> ListQuestions();
        QuestionDef GetQuestion(int id);
        void SaveNewQuestion(string name, int formatTypeId, string prompt);
        void UpdateQuestion(QuestionDef questionDef);
        void DeleteQuestion(int id);
    }
}
