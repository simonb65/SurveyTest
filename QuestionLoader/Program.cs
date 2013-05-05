using SurveyTest.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace QuestionLoader
{
    class Program
    {
        private static Dictionary<string, Action<SurveyQuestionDef>> _mappers = new Dictionary<string, Action<SurveyQuestionDef>>(StringComparer.OrdinalIgnoreCase)
        {
            { "HeaderQuestionDef", MapSimpleQuestion },
            { "MultiChoiceQuestionDef", MapMultiChoiceQuestionDef }
        };

        static void Main(string[] args)
        {
            var surveys = new InMemorySurveyRepository();
            foreach (var survey in surveys.ListSurveys())
            {
                Console.WriteLine("insert into survey(survey, survey_desc) values('{0}', '{1}')", survey.Name, survey.Description);

                foreach (var sq in survey.Questions.OrderBy(x => x.Order))
                {
                    Action<SurveyQuestionDef> mapper;
                    if (_mappers.TryGetValue(sq.QuestionDef.FormatType, out mapper))
                        mapper(sq.QuestionDef);
                    else
                        Console.WriteLine("Dunno -> {0}, {1}", sq.QuestionDef.FormatType, sq.QuestionDef.PromptText);
                }
            }
        }

        private static void MapSimpleQuestion(SurveyQuestionDef qd)
        {
            Console.WriteLine("insert into question(question_format_id, prompt_text, question_details) values(1, '{0}', null", qd.PromptText);
        }

        private static void MapMultiChoiceQuestionDef(SurveyQuestionDef qd)
        {
            var mcqd = (MultiChoiceQuestionDef)qd;

            Console.WriteLine("insert into question(question_format_id, prompt_text, question_details) values(1, '{0}', '{1}'", 
                qd.PromptText,
                string.Join(",", mcqd.QuestionTexts.Select(t => "\"" + t + "\"")));
        }
    }
}
