using System.Web.Mvc;
using Carsales.Web.Infrastructure.Attributes;
using Carsales.Web.Infrastructure.StartupTasks;
using Carsales.Web.Infrastructure.ViewEngines;

namespace Carsales.Web
{
    [AutoBind]
    public class ViewEngineSetupTask : IStartUpTask
    {
        public void Run()
        {
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new FeatureBasedRazorViewEngine());
        }
    }
}