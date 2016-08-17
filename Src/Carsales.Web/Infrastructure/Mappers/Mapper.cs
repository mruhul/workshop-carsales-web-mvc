using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Carsales.Web.Infrastructure.Attributes;

namespace Carsales.Web.Infrastructure.Mappers
{
    public interface IMapper
    {
        TOutput Map<TOutput>(object input);
    }

    [AutoBindSingleton]
    public class AutoMappedMapper : IMapper
    {
        public TOutput Map<TOutput>(object input)
        {
            return AutoMapper.Mapper.Map<TOutput>(input);
        }
    }
}