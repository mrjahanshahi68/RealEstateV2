using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Institute.Web.Models.Exam
{
    public class SectionVM
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsDeleted { get; set; }
    }
}