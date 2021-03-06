﻿drop table survey_answer;
drop table survey_question;
drop table survey_response;
drop table question_option;
drop table question_def;
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

create table question_def
(
    question_def_id int not null identity(1, 1) primary key,
    question_format_id int not null foreign key references question_format(question_format_id),
    question_def_name varchar(50) not null unique,
    question_def_description varchar(max) null,
    prompt_text varchar(255) not null,
    question_details varchar(max) null,
);

create table question_option
(
    question_option_id int not null identity(1, 1) primary key,
    question_def_id int not null foreign key references question_def(question_def_id),
    option_text varchar(255) not null,
    option_description varchar(max) null,
    option_value int null
);

create table survey_question
(
    survey_question_id int not null identity(1, 1) primary key,
    survey_id int not null foreign key references survey(survey_id),
    question_def_id int not null foreign key references question_def(question_def_id),

    question_order int not null,
    mandatory bit default 0
)

create table survey_response
(
    survey_response_id int not null identity(1, 1) primary key,
    survey_id int not null foreign key references survey(survey_id),
    date_taken datetime not null,
    person_name varchar(100),
    email_address varchar(100),
    response_total_value int
)

create table survey_answer
(
    survey_answer_id int not null identity(1, 1) primary key,
    survey_response_id int not null foreign key references survey_response(survey_response_id),
    survey_question_id int not null foreign key references survey_question(survey_question_id),
    answer varchar(255) not null,
    value int null
)