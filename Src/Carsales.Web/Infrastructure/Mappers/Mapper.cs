using Carsales.Web.Infrastructure.Attributes;

namespace Carsales.Web.Infrastructure.Mappers
{
    public interface IMapper
    {
        TOutput Map<TOutput>(object input);
    }

    [AutoBind]
    public class AutoMappedMapper : IMapper
    {
        public TOutput Map<TOutput>(object input)
        {
            return AutoMapper.Mapper.Map<TOutput>(input);
        }
    }
}