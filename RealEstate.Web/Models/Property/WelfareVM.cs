using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static RealEstate.Common.AppEnums;

namespace RealEstate.Web.Models.Property
{
    public class WelfareVM
    {
		public int ID { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
    }
}