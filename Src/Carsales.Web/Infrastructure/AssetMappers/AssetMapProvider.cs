using System;
using System.Collections.Generic;
using Bolt.Common.Extensions;
using Carsales.Web.Infrastructure.Attributes;

namespace Carsales.Web.Infrastructure.AssetMappers
{
    interface IAssetMapProvider
    {
        string Css(string name);
        string Js(string name);
    }

    [AutoBindSingleton]
    public class AssetMapProvider : IAssetMapProvider
    {
        private readonly string assetDomain;
        private readonly Lazy<IDictionary<string, AssetData>> source;
         
        public AssetMapProvider(IAssetDataReader assetDataReader)
        {
            assetDomain = System.Configuration.ConfigurationManager.AppSettings["AssetDomain"]?.TrimEnd('/') ?? string.Empty;
            source = new Lazy<IDictionary<string, AssetData>>(assetDataReader.Read);
        } 

        public string Css(string name)
        {
            return AssetUrl(source.Value.GetValueOrDefault(name)?.Css);
        }

        public string Js(string name)
        {
            return AssetUrl(source.Value.GetValueOrDefault(name)?.Js);
        }

        private string AssetUrl(string url)
        {
            if (url.IsEmpty()) return string.Empty;

            return assetDomain.HasValue() ? $"{assetDomain}/{url}" : url;
        }
    }
}
