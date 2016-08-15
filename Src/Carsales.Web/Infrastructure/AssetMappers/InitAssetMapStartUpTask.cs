using System;
using Carsales.Web.Infrastructure.Attributes;
using Carsales.Web.Infrastructure.StartupTasks;

namespace Carsales.Web.Infrastructure.AssetMappers
{
    [AutoBind]
    public class InitAssetMapStartUpTask : IStartUpTask
    {
        private readonly IAssetMapProvider _provider;

        public InitAssetMapStartUpTask(IAssetMapProvider provider)
        {
            _provider = provider;
        }

        public void Run()
        {
            _provider.Init();
        }
    }
}