using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.DataAccess
{
	public class DataContext : DbContext,IDataContext
	{
		public DataContext(string nameOrConnectionString) : base(nameOrConnectionString)
		{
            Configuration.LazyLoadingEnabled = false;
            Configuration.AutoDetectChangesEnabled = false;
            Configuration.ProxyCreationEnabled = false;
		}
	}
}
