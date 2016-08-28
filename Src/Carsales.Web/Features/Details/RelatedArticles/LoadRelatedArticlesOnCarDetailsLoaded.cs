using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bolt.Common.Extensions;
using Bolt.RequestBus;
using Carsales.Web.Features.Shared.Proxies;
using Carsales.Web.Infrastructure.Attributes;
using Carsales.Web.Infrastructure.Stores;

namespace Carsales.Web.Features.Details.RelatedArticles
{
    [AutoBind]
    public class LoadRelatedArticlesOnCarDetailsLoaded : IAsyncEventHandler<CarDetailLoadedEvent>
    {
        private readonly IContextStore<IEnumerable<RelatedArticleViewModelItem>> context;
        private readonly IEditorialSearchApiProxy apiProxy;

        public LoadRelatedArticlesOnCarDetailsLoaded(
            IContextStore<IEnumerable<RelatedArticleViewModelItem>> context,
            IEditorialSearchApiProxy apiProxy)
        {
            this.context = context;
            this.apiProxy = apiProxy;
        }

        public async Task HandleAsync(CarDetailLoadedEvent eEvent)
        {
            var response = await apiProxy.GetAsync<SearchResponseDto<dynamic>>(new EditorialSearchInput
            {
                Q = $"(Service=[CarSales]&Type=[Review]&(Make=[{eEvent.Make}]&Model=[{eEvent.Model}]))",
                Sr = "|Latest||3"
            });

            var articles = response.Output?.SearchResults.NullSafe()
                .Select(x => new RelatedArticleViewModelItem
                {
                    Title = x.Headline,
                    Photo = x.Media.Photos[0].PhotoPath
                });

            context.Set(articles);
        }
    }
}