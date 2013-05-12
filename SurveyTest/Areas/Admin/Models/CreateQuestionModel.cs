using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SurveyTest.Areas.Admin.Models
{
    public class CreateQuestionModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public int FormatId { get; set; }
        
        [Required]
        public string PromptText { get; set; }
    }
}