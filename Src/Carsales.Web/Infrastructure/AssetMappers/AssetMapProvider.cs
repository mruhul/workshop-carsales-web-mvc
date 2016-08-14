﻿using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly Lazy<IDictionary<string, AssetData>> source;
         
        public AssetMapProvider(IAssetDataReader assetDataReader)
        {
            source = new Lazy<IDictionary<string, AssetData>>(() => Load(assetDataReader));
        }

        private IDictionary<string, AssetData> Load(IAssetDataReader reader)
        {
            var basePath = System.Configuration.ConfigurationManager.AppSettings["AssetDomain"]?.TrimEnd('/') ?? string.Empty;
            var result = reader.Read();

            if (basePath.IsEmpty()) return result;

            foreach (var asset in result.Select(item => item.Value))
            {
                asset.Js = FixPathForDomain(asset.Js, basePath);
                asset.Css = FixPathForDomain(asset.Css, basePath);
            }

            return result;
        }

        public string Css(string name)
        {
            return source.Value.GetValueOrDefault(name)?.Css;
        }

        public string Js(string name)
        {
            return source.Value.GetValueOrDefault(name)?.Js;
        }
        
        private string FixPathForDomain(string path, string basePath)
        {
            if (path.IsEmpty()) return string.Empty;

            path = path.TrimStart('/');
            var sepIndex = path.IndexOf('/');

            return sepIndex == -1 ? $"{basePath}/{path}" : $"{basePath}/{path.Substring(sepIndex+1)}";
        }
    }
}