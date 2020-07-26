using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Common
{
	public static class AppConstants
	{
		public  const int ExpireMinutes = 20;
        public static class DatabaseSchema
        {
            public const string Security = "Security";
            public const string Exam = "Exam";
            public const string Log = "Log";
        }
		public static class MessageTemplate
		{
			public const string MissingToken = "Missing token";
			public const string InvalidToken = "Invalid token";
            public const string ParameterIsNotDefined = "Parameters is not defined";
			public const string InvalidIdentity = "کد شناسه نامعتبر می باشد";
            public const string Required = "{0} اجباری می باشد";
			public const string RecordNotFound = "رکورد مورد نظر یافت نشد";
			public const string TokenExpired = "توکن منقضی شده است";
		}
	}
}
