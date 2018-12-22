using System.Reflection;
using Mihaylov.Common.Mapping;

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