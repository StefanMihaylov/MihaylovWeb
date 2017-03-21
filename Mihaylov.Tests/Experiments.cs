using System.IO;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mihailov.Core;
using Mihaylov.Common.Log4net;
using Mihaylov.Common.WebConfigSettings.Interfaces;
using Mihaylov.Common.WebConfigSettings.Providers;
using Mihaylov.Core.Interfaces;
using Ninject;
using Ninject.Modules;

namespace Mihaylov.Tests
{
    [TestClass]
    public class ExperimentalTests
    {
        private static IKernel kernel;

        [ClassInitialize]
        public static void Initialize(TestContext context)
        {
            kernel = new StandardKernel();

            //var customSettingsManager = new CustomSettingsManager();

            //var loggerPath = customSettingsManager.Settings.LoggerPath;
            //var connectionString = customSettingsManager.GetSettingByName("MihaylovDb");
            //var siteUrl = customSettingsManager.GetSettingByName("SiteUrl");

            //kernel.Load(new INinjectModule[] { new NinjectModuleCore(connectionString, siteUrl) });
            //Log4netConfiguration.Setup(loggerPath);
        }

        [ClassCleanup]
        public static void Clear()
        {
            kernel.Dispose();
        }

        //[TestMethod]
        public void DeleteEmptyDirectories()
        {
            var path = @"C:\Users\Hades\AppData\Roaming\IDM\DwnlData\Hades";
            var root = new DirectoryInfo(path);

            foreach (var dir in root.GetDirectories())
            {
                if (dir.GetFiles().Length == 0)
                {
                    Directory.Delete(dir.FullName);
                }
                else
                {
                    var file = dir.GetFiles().Where(f => f.Extension != ".log").FirstOrDefault();
                    if (file != null && file.Length < 10 * 1024)
                    {
                        Directory.Delete(dir.FullName, true);
                    }
                }
            }
        }

        //[TestMethod]
        public void GetAdditionalInfo()
        {
            string username = "aaa";

            var siteHelper = kernel.Get<ISiteHelper>();
            var person = siteHelper.GetUserInfo(username);
        }

        //[TestMethod]
        public void TestManager()
        {
            var manager = kernel.Get<IAnswerTypesManager>();
            var data2 = manager.GetAllAnswerTypes().ToList();

            var unknow = manager.GetByName("notasked");

            Assert.IsNotNull(data2);
            Assert.IsTrue(data2.Count() > 0);
        }

        [TestMethod]
        public void DummyTest()
        {
            Assert.IsTrue(true);
        }
    }
}
