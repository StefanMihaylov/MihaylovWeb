using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mihaylov.Common.Mapping;
using Mihaylov.Dictionaries.Core.Interfaces;
using Mihaylov.Dictionaries.Data.Interfaces;
using Mihaylov.Dictionaries.Data.Models;
using Mihaylov.Dictionaries.Core;
using Autofac;

namespace Mihaylov.Core.Tests.Providers
{
    //[TestClass]
    public class Dicts
    {
        [TestMethod]
        public void TestMethod1()
        {
            string connectionString = "";

            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterModule(new AutofacModuleDictionariesCore(connectionString));
            var container = builder.Build();

            var mapper = new AutoMapperConfigurator(null, "Mihaylov.Data.Models");
            mapper.Execute();

            var lang = container.Resolve<IGetAllRepository<Language>>();
            var result1 = lang.GetAll().ToList();

            Assert.IsNotNull(result1);
            Assert.IsTrue(result1.Count == 1);
            Assert.AreEqual(1, result1[0].Id);
            Assert.AreEqual("English", result1[0].Name);

            var learn = container.Resolve<IGetAllRepository<LearningSystem>>();
            var result2 = learn.GetAll().ToList();

            Assert.IsNotNull(result2);
            Assert.IsTrue(result2.Count == 1);
            Assert.AreEqual(1, result2[0].Id);
            Assert.AreEqual("English File", result2[0].Name);

            var core = container.Resolve<ICoursesInfoManager>();
            var aa = core.GetAllCourses();
            var bb = core.GetAllLevels();

            var core2 = container.Resolve<IRecordsManager>();
            var rr = core2.GetAllPrepositionTypes(1);
            var ww = core2.GetAllRecordTypes();
        }
    }
}
