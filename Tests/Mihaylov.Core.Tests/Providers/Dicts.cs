using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mihaylov.Common.Mapping;
using Mihaylov.Core.Interfaces.Dictionaries;
using Mihaylov.Data;
using Mihaylov.Data.Interfaces.Dictionaries;
using Mihaylov.Data.Models.Dictionaries;
using Ninject;
using Ninject.Modules;
using DAL = Mihaylov.Database.Dictionaries;

namespace Mihaylov.Core.Tests.Providers
{
    //[TestClass]
    public class Dicts
    {
        [TestMethod]
        public void TestMethod1()
        {
            string connectionString = "";

            var kernel = new StandardKernel();
            kernel.Load(new INinjectModule[] { new NinjectModuleDictionariesCore(connectionString) });

            var mapper = new AutoMapperConfigurator(null, "Mihaylov.Data.Models");
            mapper.Execute();

            var lang = kernel.Get<IGetAllRepository<Language>>();
            var result1 = lang.GetAll().ToList();

            Assert.IsNotNull(result1);
            Assert.IsTrue(result1.Count == 1);
            Assert.AreEqual(1, result1[0].Id);
            Assert.AreEqual("English", result1[0].Name);

            var learn = kernel.Get<IGetAllRepository<LearningSystem>>();
            var result2 = learn.GetAll().ToList();

            Assert.IsNotNull(result2);
            Assert.IsTrue(result2.Count == 1);
            Assert.AreEqual(1, result2[0].Id);
            Assert.AreEqual("English File", result2[0].Name);

            var core = kernel.Get<ICoursesInfoManager>();
            var aa = core.GetAllCourses();
            var bb = core.GetAllLevels();

            var core2 = kernel.Get<IRecordsManager>();
            var rr = core2.GetAllPrepositionTypes(1);
            var ww = core2.GetAllRecordTypes();
        }
    }
}
