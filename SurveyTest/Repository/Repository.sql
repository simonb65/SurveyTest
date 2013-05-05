drop table survey_question;
drop table question;
drop table survey;
drop table question_format;

create table question_format
(
    question_format_id int not null identity(1, 1) primary key,
    question_format varchar(50) not null,
    code varchar(20) not null unique
);

insert into question_format(question_format, code) values('Header', 'Header');
insert into question_format(question_format, code) values('Simple Date', 'Date');
insert into question_format(question_format, code) values('Multiple choice', 'MultiChoice');
insert into question_format(question_format, code) values('Yes or No choice', 'YesNo');
insert into question_format(question_format, code) values('Text Result', 'Text');
insert into question_format(question_format, code) values('Number Result', 'Int');
insert into question_format(question_format, code) values('MultiSelect choice', 'MultiSelect');
insert into question_format(question_format, code) values('BloodPressure', 'BloodPressure');


create table survey
(
    survey_id int not null identity(1, 1) primary key,
    survey varchar(50) not null,
	survey_desc varchar(max)
);

create table question
(
    question_id int not null identity(1, 1) primary key,
    question_format_id int not null foreign key references question_format(question_format_id),
	prompt_text varchar(100) not null,
	question_details varchar(max) null
);

create table survey_question
(
    survey_question_id int not null identity(1, 1) primary key,
    survey_id int not null foreign key references survey(survey_id),
    question_id int not null foreign key references question(question_id),

    question_order int not null,
    mandatory bit default 0
)

