using System.Web.Mvc;

namespace Mihaylov.Web.Common
{ 
    public class ViewEngineConfig
    {
        public static void RegisterEngines()
        {
            ViewEngines.Engines.Clear();
            var viewEngine = new RazorViewEngine();

            ViewEngines.Engines.Add(viewEngine);
        }
    }
}