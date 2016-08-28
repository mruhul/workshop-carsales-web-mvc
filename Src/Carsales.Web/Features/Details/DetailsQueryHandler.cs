using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Bolt.RequestBus;
using Bolt.RequestBus.Handlers;
using Carsales.Web.Features.Shared.Proxies;
using Carsales.Web.Infrastructure.Attributes;
using Carsales.Web.Infrastructure.Mappers;
using IMapper = Carsales.Web.Infrastructure.Mappers.IMapper;

namespace Carsales.Web.Features.Details
{
    public class DetailsViewModel
    {
        public string Title { get; set; }
        public string Photo { get; set; }
        public string Price { get; set; }
    }

    public class DetailsQuery : IRequest
    {
        public string Id { get; set; }
    }

    public class DetailsApiDto
    {
        public string Title { get; set; }
        public string Photo { get; set; }
        public double? Price { get; set; }
        public string PriceType { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Badge { get; set; }
    }

    public class CarDetailLoadedEvent : IEvent
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Photo { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Year { get; set; }
        public string Badge { get; set; }
    }

    [AutoBind]
    public class MappingSetupTask : IMappingSetupTask
    {
        public void Run(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<DetailsApiDto, CarDetailLoadedEvent>();
            cfg.CreateMap<DetailsApiDto, DetailsViewModel>();
        }
    }

    [AutoBind]
    public class DetailsQueryHandler : AsyncRequestHandlerBase<DetailsQuery, DetailsViewModel>
    {
        private readonly ICarDetailsApiProxy apiProxy;
        private readonly IRequestBus bus;
        private readonly IMapper mapper;

        public DetailsQueryHandler(ICarDetailsApiProxy apiProxy, IRequestBus bus, IMapper mapper)
        {
            this.apiProxy = apiProxy;
            this.bus = bus;
            this.mapper = mapper;
        }
        
        protected override async Task<DetailsViewModel> ProcessAsync(DetailsQuery msg)
        {
            var taskPageRequested = bus.PublishAsync(new DetailsPageRequestedEvent());

            var taskDetails = LoadDetailsAsync(msg.Id);

            await Task.WhenAll(taskPageRequested, taskDetails);

            return taskDetails.Result;
        }

        private async Task<DetailsViewModel> LoadDetailsAsync(string id)
        {
            var response = await apiProxy.GetAsync<IEnumerable<DetailsApiDto>>(new CarDetailsInput
            {
                Ids = id,
                Projection = "title:Specification.Title,photo:Media.Photos[0].PhotoPath,make:Specification.Make,model:Specification.Model,badge:Specification.Badge"
            });

            var dto = response.Output?.FirstOrDefault();

            if (dto == null) return null;

            await bus.PublishAsync(mapper.Map<CarDetailLoadedEvent>(dto));

            return mapper.Map<DetailsViewModel>(dto);
        }
    }
}