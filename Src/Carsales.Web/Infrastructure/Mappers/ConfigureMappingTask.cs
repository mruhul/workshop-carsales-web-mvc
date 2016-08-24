using System;
using System.Collections.Generic;
using AutoMapper;
using Bolt.Common.Extensions;
using Carsales.Web.Infrastructure.Attributes;
using Carsales.Web.Infrastructure.StartupTasks;

namespace Carsales.Web.Infrastructure.Mappers
{
    [AutoBind]
    public class ConfigureMappingTask : IStartUpTask
    {
        private readonly IEnumerable<IMappingSetupTask> mappingSetupTasks;

        public ConfigureMappingTask(IEnumerable<IMappingSetupTask> mappingSetupTasks)
        {
            this.mappingSetupTasks = mappingSetupTasks;
        }

        public void Run()
        {
            Mapper.Initialize(cfg =>
            {
                mappingSetupTasks.ForEach(x =>
                {
                    x.Run(cfg);
                });
            });

            Mapper.AssertConfigurationIsValid();
        }
    }
}