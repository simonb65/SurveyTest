//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SurveyTest.Repository
{
    using System;
    using System.Collections.Generic;
    
    public partial class question_def
    {
        public question_def()
        {
            this.QuestionOptions = new HashSet<question_option>();
        }
    
        public int question_def_id { get; set; }
        public int question_format_id { get; set; }
        public string prompt_text { get; set; }
        public string question_details { get; set; }
    
        public virtual ICollection<question_option> QuestionOptions { get; set; }
        public virtual question_format QuestionFormat { get; set; }
    }
}