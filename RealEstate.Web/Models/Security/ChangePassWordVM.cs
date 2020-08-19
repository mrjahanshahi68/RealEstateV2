using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using static RealEstate.Common.AppEnums;

namespace RealEstate.Web.Models.Security
{
    public class ChangePassWordVM
    {
		public int UserId { get; set; }
        public string NewPass { get; set; }
        public string RepairNewPass { get; set; }
    }
}