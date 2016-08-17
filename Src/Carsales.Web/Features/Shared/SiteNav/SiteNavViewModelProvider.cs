using Carsales.Web.Infrastructure.Attributes;
using Carsales.Web.Infrastructure.Stores;

namespace Carsales.Web.Features.Shared.SiteNav
{
    public interface ISiteNavViewModelProvider
    {
        SiteNavViewModel Get();
        void Set(SiteNavViewModel value);
    }

    [AutoBind]
    public class SiteNavViewModelProvider : ISiteNavViewModelProvider
    {
        private readonly IContextStore store;
        private const string Key = "SiteNavViewModelProvider.SiteNavViewModel";

        public SiteNavViewModelProvider(IContextStore store)
        {
            this.store = store;
        }

        public SiteNavViewModel Get()
        {
            return store.Get<SiteNavViewModel>(Key) ?? new SiteNavViewModel();
        }

        public void Set(SiteNavViewModel value)
        {
            store.Set(Key, value);
        }
    }
}