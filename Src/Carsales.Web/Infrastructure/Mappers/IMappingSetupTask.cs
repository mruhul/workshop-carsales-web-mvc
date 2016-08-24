using AutoMapper;

namespace Carsales.Web.Infrastructure.Mappers
{
    public interface IMappingSetupTask
    {
        void Run(IMapperConfigurationExpression cfg);
    }
}