using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Institute.Web.Models.Exam
{
    public class QuestionVM
    {
        public int ID { get; set; }
        public string Subject { get; set; }
        public string Answer_1 { get; set; }
        public string Answer_2 { get; set; }
        public string Answer_3 { get; set; }
        public string Answer_4 { get; set; }
        public int CorrectAnswer { get; set; }
        public bool IsDeleted { get; set; }
    }
}