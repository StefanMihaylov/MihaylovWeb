using Mihaylov.Common.Mapping;
using System.Reflection;

namespace Mihaylov.Web.Common
{
    public class AutoMapperConfig
    {
        public static void RegisterModels()
        {
            var autoMapper = new AutoMapperConfigurator(
                Assembly.GetExecutingAssembly(), 
                "Mihaylov.Data.Models"
                );

            autoMapper.Execute();
        }
    }
}