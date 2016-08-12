using System.Diagnostics;

namespace Carsales.Web.Infrastructure.Stores
{
    public static class ContextStoreExtensions
    {
        [DebuggerStepThrough]
        public static T Get<T>(this IContextStore source, string name)
        {
            return (T) source.Get(name);
        }
    }
}