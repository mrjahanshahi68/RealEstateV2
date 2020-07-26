using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Web.Infrastrcuture
{
    public class DataStore
    {
        private readonly Dictionary<string, object> _properties;
        public DataStore()
        {
            this._properties = new Dictionary<string, object>();
        }
        private void SetProperty<T>(string name, T value)
        {
            _properties.Add(name, value);
        }
        private dynamic GetProperty(string name)
        {
            dynamic value;
            _properties.TryGetValue(name, out value);
            return value;
        }
        public dynamic this[string index]
        {
            get { return GetProperty(index); }
            set { SetProperty(index, value); }
        }
    }
}
