using System.Collections.Concurrent;
using System.Diagnostics;
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

        [DebuggerStepThrough]
        public object Get(string name)
        {
            object result;
            return store.TryGetValue(name, out result) ? result : null;
        }

        [DebuggerStepThrough]
        public void Set(string name, object value)
        {
            store.TryAdd(name, value);
        }
    }

    public interface IContextStore<T>
    {
        T Get();
        void Set(T value);
    }
    
    public class ContextStore<T> : IContextStore<T>
    {
        private T value;

        public ContextStore()
        {
            value = default(T);
        }
        
        public T Get()
        {
            return value;
        }

        public void Set(T data)
        {
            this.value = data;
        }
    }
}