
declare @SurveyId int;
declare @QuestionId int;
insert into survey(survey, survey_desc) values('Remedy HRA', 'Welcome to the Remedy Healthcare risk assessment tool');
select @SurveyId = @@IDENTITY;
insert into question_def(question_format_id, prompt_text, question_details) values(1, 'Personal Details', null);
select @QuestionId = @@IDENTITY;
insert into survey_question(survey_id, question_def_id, question_order, mandatory) values(@SurveyId, @QuestionId, 1, 1);
insert into question_def(question_format_id, prompt_text, question_details) values(8, 'Blood Pressure', null);
select @QuestionId = @@IDENTITY;
insert into survey_question(survey_id, question_def_id, question_order, mandatory) values(@SurveyId, @QuestionId, 2, 1);
insert into question_def(question_format_id, prompt_text, question_details) values(6, 'Total Cholesterol', '<opt><qt></qt><qs>mmol/L (if known)</qs><sl>True</sl></opt>');
select @QuestionId = @@IDENTITY;
insert into survey_question(survey_id, question_def_id, question_order, mandatory) values(@SurveyId, @QuestionId, 3, 1);
insert into question_def(question_format_id, prompt_text, question_details) values(6, 'Blood Glucose', '<opt><qt></qt><qs>mmol/L (if known)</qs><sl>True</sl></opt>');
select @QuestionId = @@IDENTITY;
insert into survey_question(survey_id, question_def_id, question_order, mandatory) values(@SurveyId, @QuestionId, 4, 1);
insert into question_def(question_format_id, prompt_text, question_details) values(7, 'Current medical conditions', '<opts><opt>Heart Disease</opt><opt>High cholesterol</opt><opt>Type 2 diabetes</opt><opt>Asthma</opt><opt>Overweight or obesity</opt><opt>Hayfever</opt><opt>Anxiety or depression</opt><opt>Back pain or other chronic pain</opt><opt>High blood pressure</opt></opts>');
select @QuestionId = @@IDENTITY;
insert into survey_question(survey_id, question_def_id, question_order, mandatory) values(@SurveyId, @QuestionId, 5, 1);
insert into question_def(question_format_id, prompt_text, question_details) values(2, '1. Date of birth', null);
select @QuestionId = @@IDENTITY;
insert into survey_question(survey_id, question_def_id, question_order, mandatory) values(@SurveyId, @QuestionId, 6, 1);
insert into question_def(question_format_id, prompt_text, question_details) values(3, '2. Your Gender?', '<opts><opt>Female</opt><opt>Male</opt></opts>');
select @QuestionId = @@IDENTITY;
insert into survey_question(survey_id, question_def_id, question_order, mandatory) values(@SurveyId, @QuestionId, 7, 1);
insert into question_def(question_format_id, prompt_text, question_details) values(3, '3. Your ethnicity/country of birth?', '<opts><opt>Aboriginal, Torres Strait Islander, Pacific Islander or Maori</opt><opt>Australia</opt><opt>Asia (including the Indian sub-continent), Middle East, North Africa, Southern Europe</opt><opt>Other</opt></opts>');
select @QuestionId = @@IDENTITY;
insert into survey_question(survey_id, question_def_id, question_order, mandatory) values(@SurveyId, @QuestionId, 8, 1);
insert into question_def(question_format_id, prompt_text, question_details) values(4, '4. Have either of your parents, or any of your brothers or sisters been diagnosed with diabetes (type 1 or type 2)?', null);
select @QuestionId = @@IDENTITY;
insert into survey_question(survey_id, question_def_id, question_order, mandatory) values(@SurveyId, @QuestionId, 9, 1);
insert into question_def(question_format_id, prompt_text, question_details) values(4, '5.  Have you ever been found to have high blood glucose (blood sugar) (for example, in a health examination, during an illness, during pregnancy)?', null);
select @QuestionId = @@IDENTITY;
insert into survey_question(survey_id, question_def_id, question_order, mandatory) values(@SurveyId, @QuestionId, 10, 1);
insert into question_def(question_format_id, prompt_text, question_details) values(4, '6. Are you currently taking medication for high blood pressure?', null);
select @QuestionId = @@IDENTITY;
insert into survey_question(survey_id, question_def_id, question_order, mandatory) values(@SurveyId, @QuestionId, 11, 1);
insert into question_def(question_format_id, prompt_text, question_details) values(4, '7. Do you currently smoke cigarettes or any other tobacco products on a daily basis?', null);
select @QuestionId = @@IDENTITY;
insert into survey_question(survey_id, question_def_id, question_order, mandatory) values(@SurveyId, @QuestionId, 12, 1);
insert into question_def(question_format_id, prompt_text, question_details) values(3, '8.  How often do you eat vegetables or fruit?', '<opts><opt>Every day</opt><opt>Not every day</opt></opts>');
select @QuestionId = @@IDENTITY;
insert into survey_question(survey_id, question_def_id, question_order, mandatory) values(@SurveyId, @QuestionId, 13, 1);
insert into question_def(question_format_id, prompt_text, question_details) values(4, '9.  On average, would you say you do at least 2.5 hours of physical activity per week (for example, 30 minutes a day on 5 or more days a week)?', null);
select @QuestionId = @@IDENTITY;
insert into survey_question(survey_id, question_def_id, question_order, mandatory) values(@SurveyId, @QuestionId, 14, 1);
insert into question_def(question_format_id, prompt_text, question_details) values(6, '10.  Your waist measurement taken below the ribs (usually at the level of the navel, and while standing)', '<opt><qt>Waist measurement (cm)</qt><qs></qs><sl>True</sl></opt>');
select @QuestionId = @@IDENTITY;
insert into survey_question(survey_id, question_def_id, question_order, mandatory) values(@SurveyId, @QuestionId, 15, 1);


insert into survey(survey, survey_desc) values('AU Staff HRA', 'Welcome to the Australian Unity Staff Health Risk Assessment tool');
insert into survey(survey, survey_desc) values('HBA HRA', 'Welcome to the HAB Health Risk Assessment tool');


select * from survey
select * from question_def
select * from survey_question

select
	q.prompt_text, a.survey_answer, f.question_format
from
	survey_answer a 
	inner join survey_question sq on a.survey_question_id = sq.survey_question_id
	inner join question_def q on q.question_def_id = sq.question_def_id
	inner join question_format f on q.question_format_id = f.question_format_id
order by
	sq.question_order






















































