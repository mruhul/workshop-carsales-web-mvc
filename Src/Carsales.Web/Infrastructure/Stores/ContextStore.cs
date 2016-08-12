using System.Collections.Concurrent;
using Carsales.Web.Infrastructure.Attributes;

namespace Carsales.Web.Infrastructure.Stores
{
    public interface IContextStore
    {
        object Get(string name);
        void Set(string name, object value);
    }
    
    [AutoBindPerRequest]
    public class ContextStore : IContextStore
    {
        private readonly ConcurrentDictionary<string, object> store;

        public ContextStore()
        {
            this.store = new ConcurrentDictionary<string, object>();
        }

        public object Get(string name)
        {
            object result;
            return store.TryGetValue(name, out result) ? result : null;
        }

        public void Set(string name, object value)
        {
            store.TryAdd(name, value);
        }
    }
}