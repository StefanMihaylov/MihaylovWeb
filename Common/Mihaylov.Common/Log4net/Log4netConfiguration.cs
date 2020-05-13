using System.Reflection;
using log4net;
using log4net.Appender;
using log4net.Layout;
using log4net.Repository.Hierarchy;

namespace Mihaylov.Common.Log4net
{
    public class Log4netConfiguration
    {
        public static void Setup(string filePath)
        {
            Hierarchy hierarchy = (Hierarchy)LogManager.GetRepository(Assembly.GetEntryAssembly());

            var patternLayout = new PatternLayout
            {
                ConversionPattern = "%date [%thread] %-5level %logger - %message%newline"
            };

            patternLayout.ActivateOptions();

            var roller = new RollingFileAppender
            {
                AppendToFile = true,
                File = filePath,
                Layout = patternLayout,
                MaxSizeRollBackups = 10,
                MaximumFileSize = "5MB",
                RollingStyle = RollingFileAppender.RollingMode.Date,//.Size;
                StaticLogFileName = true
            };

            roller.ActivateOptions();

            hierarchy.Root.AddAppender(roller);

            MemoryAppender memory = new MemoryAppender();
            memory.ActivateOptions();
            hierarchy.Root.AddAppender(memory);

            hierarchy.Root.Level = log4net.Core.Level.All;
            hierarchy.Configured = true;

            // LogentriesAppender
            //PatternLayout entityPatternLayout = new PatternLayout();
            //entityPatternLayout.ConversionPattern = "%date [%thread] %-5level %logger - %message%newline";

            //LogentriesAppender entity = new LogentriesAppender();
            //entity.Layout = entityPatternLayout;
            //entity.Debug = true;
            //hierarchy.Root.AddAppender(entity);
        }
    }
}
