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
        public string CurrentUserId => "4eaa1c60-5871-4f9c-b825-7e5f1ed7b892";
        public bool IsAuthenticated => true;
    }
}