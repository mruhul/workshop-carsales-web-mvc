using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Carsales.Web.Infrastructure.Attributes
{
    public class AutoBindAttribute : Attribute { }
    public class AutoBindSelfAttribute : Attribute { }
    public class AutoBindSingletonAttribute : Attribute { }
    public class AutoBindPerRequestAttribute : Attribute { }
}