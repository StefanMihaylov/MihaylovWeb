using System.Web.Mvc;

namespace Mihaylov.Web.App_Start
{
    public class ViewEngineConfig
    {
        public static void RegisterEngines()
        {
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());
        }
    }
}