using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Web.Infrastrcuture
{
    public sealed class SessionStorage
    {
        private static readonly Lazy<SessionStorage> lazy =
            new Lazy<SessionStorage>(() => new SessionStorage());
        private readonly ConcurrentDictionary<string, object> _properties;
        public static SessionStorage Instance { get { return lazy.Value; } }
        private SessionStorage()
        {
            _properties = new ConcurrentDictionary<string, object>();
        }
        private void SetProperty<T>(string name, T value)
        {
            _properties.TryAdd(name, value);
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
