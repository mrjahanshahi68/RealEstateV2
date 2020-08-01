using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RealEstate.Web.Models.Common
{
    public class MessageVM
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Text { get; set; }
        public bool IsDeleted { get; set; }
    }
}