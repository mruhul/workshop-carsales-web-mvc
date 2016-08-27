using System.Collections.Generic;
using AutoMapper;
using Bolt.Common.Extensions;
using Carsales.Web.Infrastructure.Attributes;
using Carsales.Web.Infrastructure.StartupTasks;

namespace Carsales.Web.Infrastructure.Mappers
{
    [AutoBind]
    public class SetupMappingStartUpTask : IStartUpTask
    {
        private readonly IEnumerable<IMappingSetupTask> setupTasks;

        public SetupMappingStartUpTask(IEnumerable<IMappingSetupTask> setupTasks)
        {
            this.setupTasks = setupTasks;
        }

        public void Run()
        {
            Mapper.Initialize(cfg =>
            {
                setupTasks.ForEach(x => x.Run(cfg));
            });

            Mapper.AssertConfigurationIsValid();
        }
    }
}