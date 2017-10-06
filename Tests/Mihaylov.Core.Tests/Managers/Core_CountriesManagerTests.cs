using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mihaylov.Core.Interfaces.Site;
using Mihaylov.Core.Managers.Site;
using Mihaylov.Core.Tests.Managers.Fakes;
using Mihaylov.Data.Models.Site;
using Ninject.Extensions.Logging;
using Telerik.JustMock;

namespace Mihaylov.Core.Tests.Managers
{
    [TestClass]
    public class Core_CountriesManagerTests
    {
        private const int NUMBER_OF_COUNTRIES = 5;
        private const string NAME_TEMPLATE = "Country {0}";
        private const string VALID_NAME = "Country 3";

        [TestMethod]
        public void ConstructiorProviderNullTest()
        {
            ILogger logger = Mock.Create<ILogger>();

            Assert.ThrowsException<ArgumentNullException>(() => new CountriesManager(null, logger));
        }

        [TestMethod]
        public void ConstructiorProviderIsSetProperly()
        {
            ICountriesProvider providerMock = this.GetCountriesProvider();
            ILogger loggerMock = Mock.Create<ILogger>();
            var manager = new CountriesManagerFake(providerMock, loggerMock);

            Assert.AreSame(providerMock, manager.ExposedProvider);
        }

        [TestMethod]
        public void ConstructiorLoggerNullTest()
        {
            ICountriesProvider provider = Mock.Create<ICountriesProvider>();

            Assert.ThrowsException<ArgumentNullException>(() => new CountriesManager(provider, null));
        }

        [TestMethod]
        public void ConstructiorLoggerIsSetProperly()
        {
            ICountriesProvider providerMock = this.GetCountriesProvider();
            ILogger loggerMock = Mock.Create<ILogger>();
            var manager = new CountriesManagerFake(providerMock, loggerMock);

            Assert.AreSame(loggerMock, manager.ExposedLogger);
        }

        [TestMethod]
        [DataRow(2, DisplayName = "Reqular Id, start")]
        [DataRow(NUMBER_OF_COUNTRIES / 2, DisplayName = "Reqular Id, middle")]
        [DataRow(NUMBER_OF_COUNTRIES, DisplayName = "Reqular Id, end")]
        public void GeCountryByIdTest(int id)
        {
            ILogger logger = Mock.Create<ILogger>();
            ICountriesProvider provider = GetCountriesProvider();
            var manager = new CountriesManagerFake(provider, logger);

            Country result = manager.GetById(id);

            Assert.IsNotNull(result);
            Assert.AreEqual(id, result.Id);
            Assert.IsNotNull(result.Name);

            Assert.AreEqual(1, manager.ExposedDictionaryById.Count);
        }

        [TestMethod]
        [DataRow(NUMBER_OF_COUNTRIES + 1, DisplayName = "Id, out of range")]
        public void GetCountryByIdInvalidIdTest(int id)
        {
            ILogger logger = Mock.Create<ILogger>();
            ICountriesProvider provider = GetCountriesProvider();
            var manager = new CountriesManagerFake(provider, logger);

            Country result = manager.GetById(id);

            Assert.IsNull(result);

            Assert.AreEqual(0, manager.ExposedDictionaryById.Count);
        }

        [TestMethod]
        [DataRow(0, DisplayName = "Zero Id")]
        [DataRow(-3, DisplayName = "Negative Id")]
        public void GetCountryByIdIncorrectTest(int id)
        {
            ILogger logger = Mock.Create<ILogger>();
            ICountriesProvider provider = GetCountriesProvider();
            ICountriesManager manager = new CountriesManager(provider, logger);

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => manager.GetById(id));
        }

        [TestMethod]
        [DataRow(VALID_NAME, VALID_NAME, DisplayName = "Valid name")]
        [DataRow(VALID_NAME, "  " + VALID_NAME + "   ", DisplayName = "Name with white spaces")]
        public void GetCountryByNameTest(string name, string inputName)
        {
            ILogger logger = Mock.Create<ILogger>();
            ICountriesProvider provider = GetCountriesProvider();
            var manager = new CountriesManagerFake(provider, logger);

            Country result = manager.GetByName(inputName);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Id > 0);
            Assert.AreEqual(name, result.Name);

            Assert.AreEqual(1, manager.ExposedDictionaryByName.Count);
        }

        [TestMethod]
        public void GetCountryByNameUpperCaseTest()
        {
            string validName = string.Format(NAME_TEMPLATE, 3);
            string name = validName.ToUpper();

            ILogger logger = Mock.Create<ILogger>();
            ICountriesProvider provider = GetCountriesProvider();
            var manager = new CountriesManagerFake(provider, logger);

            Country result = manager.GetByName(name);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Id > 0);
            Assert.AreEqual(validName, result.Name);

            Assert.AreEqual(1, manager.ExposedDictionaryByName.Count);
        }

        [TestMethod]
        public void GetCountryByNameLowerCaseTest()
        {
            string validName = string.Format(NAME_TEMPLATE, 3);
            string name = validName.ToLower();

            ILogger logger = Mock.Create<ILogger>();
            ICountriesProvider provider = GetCountriesProvider();
            var manager = new CountriesManagerFake(provider, logger);

            Country result = manager.GetByName(name);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Id > 0);
            Assert.AreEqual(validName, result.Name);

            Assert.AreEqual(1, manager.ExposedDictionaryByName.Count);
        }

        [TestMethod]
        public void GetCountryByNameInvalidNameTest()
        {
            string name = "abc";

            ILogger logger = Mock.Create<ILogger>();
            ICountriesProvider provider = GetCountriesProvider();
            var manager = new CountriesManagerFake(provider, logger);

            Country result = manager.GetByName(name);

            Assert.IsNull(result);

            Assert.AreEqual(0, manager.ExposedDictionaryByName.Count);
        }

        [TestMethod]
        public void RequestByNameIsLoggedTest()
        {
            string name = VALID_NAME;

            ILogger logger = Mock.Create<ILogger>();
            ICountriesProvider provider = GetCountriesProvider();
            var manager = new CountriesManagerFake(provider, logger);

            Country result = manager.GetByName(name);

            Mock.Assert(() => logger.Debug(Arg.AnyString), Occurs.AtLeastOnce());
        }

        [TestMethod]
        public void RequestByNameCallProviderByNameTest()
        {
            string name = VALID_NAME;

            ILogger logger = Mock.Create<ILogger>();
            ICountriesProvider provider = GetCountriesProvider();
            var manager = new CountriesManagerFake(provider, logger);

            Country result = manager.GetByName(name);

            Mock.Assert(() => provider.GetByName(Arg.AnyString), Occurs.Once());
        }

        [TestMethod]
        public void RequestByIdCallProviderByIdTest()
        {
            int id = 2;

            ILogger logger = Mock.Create<ILogger>();
            ICountriesProvider provider = GetCountriesProvider();
            var manager = new CountriesManagerFake(provider, logger);

            Country result = manager.GetById(id);

            Mock.Assert(() => provider.GetById(Arg.AnyInt), Occurs.Once());
        }

        private ICountriesProvider GetCountriesProvider()
        {
            var countries = new List<Country>();
            for (int index = 1; index <= NUMBER_OF_COUNTRIES; index++)
            {
                countries.Add(new Country()
                {
                    Id = index,
                    Name = string.Format(NAME_TEMPLATE, index),
                    Description = string.Format("N/A {0}", index)
                });
            }

            var provider = Mock.Create<ICountriesProvider>();

            Mock.Arrange(() => provider.GetById(Arg.AnyInt)).Returns<int>((id) =>
             {
                 Country result = countries.FirstOrDefault(c => c.Id == id);
                 if (result == null)
                 {
                     throw new ApplicationException();
                 }

                 return result;
             });

            Mock.Arrange(() => provider.GetByName(Arg.AnyString)).Returns<string>((name) =>
            {
                Country result = countries.FirstOrDefault(c => c.Name.ToLower() == name.ToLower());
                if (result == null)
                {
                    throw new ApplicationException();
                }

                return result;
            });

            Mock.Arrange(() => provider.GetAll()).Returns(countries);

            return provider;
        }
    }
}
