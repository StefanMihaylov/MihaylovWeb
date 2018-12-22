using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mihaylov.Site.Core.Interfaces;
using Ninject;

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
        public void GetAdditionalInfo()
        {
            string username = "aaa";

            var siteHelper = kernel.Get<ISiteHelper>();
            var person = siteHelper.GetUserInfo(username);
        }

        //[TestMethod]
        public void TestManager()
        {
            var manager = kernel.Get<IPersonAdditionalInfoManager>();
            var data2 = manager.GetAllAnswerTypes().ToList();

            var unknow = manager.GetAnswerTypeByName("notasked");

            Assert.IsNotNull(data2);
            Assert.IsTrue(data2.Count() > 0);
        }
    }
}
