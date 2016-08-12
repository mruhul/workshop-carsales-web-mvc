using System;
using System.Collections.Generic;
using System.IO;
using Bolt.Serializer;
using Carsales.Web.Infrastructure.Attributes;

namespace Carsales.Web.Infrastructure.AssetMappers
{
    public interface IAssetDataReader
    {
        IDictionary<string, AssetData> Read();
    }

    [AutoBind]
    public class AssetDataReader : IAssetDataReader
    {
        private readonly ISerializer serializer;

        public AssetDataReader(ISerializer serializer)
        {
            this.serializer = serializer;
        }

        public IDictionary<string, AssetData> Read()
        {
            var content = File.ReadAllText($"{AppDomain.CurrentDomain.BaseDirectory}/webpack.assets.json");
            return serializer.Deserialize<IDictionary<string, AssetData>>(content);
        }
    }
}