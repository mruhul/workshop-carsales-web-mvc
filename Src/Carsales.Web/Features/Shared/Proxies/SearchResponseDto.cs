using System.Collections.Generic;
using Expresso.Navigation;

namespace Carsales.Web.Features.Shared.Proxies
{
    public class SearchResponseDto<T>
    {
        public IEnumerable<T> SearchResults { get; set; }
        public long Count { get; set; }
    }

    public class SelectListResponseDto
    {
        public int Count { get; set; }
        public SelectListCollection SelectListCollection { get; set; }
    }
}