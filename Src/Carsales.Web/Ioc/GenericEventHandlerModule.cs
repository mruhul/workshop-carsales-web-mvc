﻿using Autofac;
using Bolt.RequestBus;
//using Carsales.Web.Features.Shared.SavedCars;
using Carsales.Web.Features.Shared.SiteNav;

namespace Carsales.Web.Ioc
{
    public class GenericEventHandlerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(LoadSiteNavOnPageLoadEventHandler<>)).As(typeof(IAsyncEventHandler<>));
            //builder.RegisterGeneric(typeof(LoadCurrentUserSavedCarsOnPageLoad<>)).As(typeof(IAsyncEventHandler<>));
        }
    }
}