using RealEstate.Common.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RealEstate.Common.AppConstants;

namespace RealEstate.DataAccess.Common.MapConfigurations
{
	public class BlogMapConfig : LoggableEntityMapConfig<Blog>
	{
		public BlogMapConfig()
		{
			Property(e => e.Title);
			Property(e => e.Body);
			Property(e => e.Summery);
			Property(e => e.SlugUrl);
			Property(e => e.CategoryId);
			Property(e => e.Discriminator);
			Property(e => e.MetaDescription);
			Property(e => e.Sort);
			Property(e => e.ViewNumber);
			Property(e => e.IsActive);
			Property(e => e.IsDeleted);
			Property(e => e.PublishDateTime);

		ToTable("Blogs");
		}
	}
}
