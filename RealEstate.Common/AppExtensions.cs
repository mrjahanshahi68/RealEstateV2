using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Common
{
	public static class AppExtensions
	{
		public static byte[] ToMd5(this string text)
		{
			var md5 = MD5.Create();
			var textArray = Encoding.UTF32.GetBytes(text);
			var hash = md5.ComputeHash(textArray);
			return hash;
		}
		public static string ToBase64(this byte[] byteArray)
		{
			return Convert.ToBase64String(byteArray);
		}
        public static string GetApiAction(this Uri uri)
        {
            try
            {
                return uri.Segments[uri.Segments.Length - 1].Trim('/');
            }
            catch
            {
                return "UnknownAction";
            }
        }
        public static string ToTimeStamp(this DateTime date)
        {
            return date.ToString("yyyyMMddHHmmssffff");
        }
        public static string ToShamsi(this DateTime date)
        {
            PersianCalendar pc = new PersianCalendar();
            return $"{pc.GetYear(date)}/{pc.GetMonth(date)}/{pc.GetDayOfMonth(date)}";
        }
	}
}
