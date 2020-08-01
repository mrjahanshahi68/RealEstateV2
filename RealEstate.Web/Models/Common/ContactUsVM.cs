using System.ComponentModel.DataAnnotations;

namespace RealEstate.Web.Models.Common
{
    public class ContactUsVM
    {
        public int ID { get; set; }
		public string Title { get; set; }
		public string LinkUrlInstagram { get; set; }
		public string LinkUrlWhatsapp { get; set; }
		public string LinkUrlTelegram { get; set; }
		public string LinkUrlPinterest { get; set; }
		public string LinkUrlGoogle { get; set; }

		[Required(ErrorMessage ="لطفا شماره موبایل را وارد نمائید")]
		[StringLength(11,ErrorMessage ="شماره موبایل حداکثر می تواند 11 کارکتر باشد.",MinimumLength =11)]
		public string MobileNumber { get; set; }

		[Required(ErrorMessage = "لطفا تلفن ثابت را وارد نمائید")]
		[StringLength(11, ErrorMessage = "تلفن ثابت حداکثر می تواند 11 کارکتر باشد.", MinimumLength = 11)]

		public string PhoneNumber { get; set; }
		public string Address { get; set; }
		public bool IsDeleted { get; set; }
	}
}