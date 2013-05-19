using SurveyTest.Models;
using SurveyTest.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace QuestionLoader
{
    public static class DbConst
    {
        public const int HeaderFormatId = 1;
        public const int DateFormatId = 2;
        public const int MultiChoiceFormatId = 3;
        public const int YesNoFormatId = 4;
        public const int TextFormatId = 5;
        public const int IntFormatId = 6;
        public const int MultiSelectFormatId = 7;
        public const int BloodPressureFormatId = 8;
    }

    class Program
    {

        static void Main(string[] args)
        {
            var surveys = new InMemorySurveyRepository();

            Console.WriteLine("declare @SurveyId int;");
            Console.WriteLine("declare @QuestionId int;");


            foreach (var survey in surveys.ListSurveys())
            {
                Console.WriteLine("insert into survey(survey, survey_desc) values('{0}', '{1}');", survey.Name, survey.Description);
                Console.WriteLine("select @SurveyId = @@IDENTITY;");

                int questionOrder = 1;

                foreach (var sq in survey.Questions.OrderBy(x => x.Order))
                {
                    string serialisedData = null;
                    int formatId = 0;

                    switch (sq.Question.GetType().Name)
                    {
                        case "HeaderQuestionDef":
                            formatId = DbConst.HeaderFormatId;
                            break;
                        case "DateQuestionDef":
                            formatId = DbConst.DateFormatId;
                            break;
                        case "MultiChoiceQuestionDef":
                            // serialisedData = MapMultiChoiceSerialisedData(sq.QuestionDef);
                            formatId = DbConst.MultiChoiceFormatId;
                            break;
                        case "YesNoSurveyQuestion":
                            formatId = DbConst.YesNoFormatId;
                            break;
                        case "TextQuestionDef":
                            formatId = DbConst.TextFormatId;
                            serialisedData = MapTextSerialisedData(sq.Question);
                            break;
                        case "IntQuestionDef":
                            formatId = DbConst.IntFormatId;
                            serialisedData = MapTextSerialisedData(sq.Question);
                            break;
                        case "MultiSelectQuestionDef":
                            formatId = DbConst.MultiSelectFormatId;
                            // serialisedData = MapMultiSelectSerialisedData(sq.QuestionDef);
                            break;
                        case "BloodPressureQuestionDef":
                            formatId = DbConst.BloodPressureFormatId;
                            break;
                    }

                    Console.WriteLine(
                        "insert into question_def(question_format_id, prompt_text, question_details) values({0}, '{1}', {2});", 
                        formatId,
                        sq.Question.PromptText,
                        (serialisedData == null) ? "null" : ("'" + serialisedData + "'"));

                    Console.WriteLine("select @QuestionId = @@IDENTITY;");
                    Console.WriteLine("insert into survey_question(survey_id, question_id, question_order, mandatory) values(@SurveyId, @QuestionId, {0}, 1);", questionOrder++);
                }
            }
        }

        private static string MapTextSerialisedData(QuestionDef sqd)
        {
            var tqd = (TextQuestionDef)sqd;
            return string.Format("<opt><qt>{0}</qt><qs>{1}</qs><sl>{2}</sl></opt>", tqd.QuestionText, tqd.QuestionSuffix, tqd.SingleLine);
        }
    }
}
