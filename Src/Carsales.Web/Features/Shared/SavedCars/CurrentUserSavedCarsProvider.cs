using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bolt.Common.Extensions;
using Carsales.Web.Infrastructure.Attributes;
using Carsales.Web.Infrastructure.Stores;

namespace Carsales.Web.Features.Shared.SavedCars
{
    public interface ICurrentUserSavedCarsProvider
    {
        IEnumerable<string> Get();
        void Set(IEnumerable<string> networkIds);
    }

    [AutoBind]
    public class CurrentUserSavedCarsProvider : ICurrentUserSavedCarsProvider
    {
        private readonly IContextStore store;
        private const string Key = "CurrentUserSavedCarsProvider";

        public CurrentUserSavedCarsProvider(IContextStore store)
        {
            this.store = store;
        }

        public IEnumerable<string> Get()
        {
            return store.Get<IEnumerable<string>>(Key).NullSafe();
        }

        public void Set(IEnumerable<string> networkIds)
        {
            store.Set(Key, networkIds);
        }
    }
}