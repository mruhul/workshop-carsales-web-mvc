using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Carsales.Web.Infrastructure.Attributes;

namespace Carsales.Web.Infrastructure.UserContext
{
    public interface IUserContext
    {
        string CurrentUserId { get; }
        bool IsAuthenticated { get; }
    }

    [AutoBindPerRequest]
    public class UserContext : IUserContext
    {
        public string CurrentUserId => "1c28b1ff-3a9a-466e-a0f7-13b72af3bdef";
        public bool IsAuthenticated => true;
    }
}