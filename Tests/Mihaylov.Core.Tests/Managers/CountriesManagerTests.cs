using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mihaylov.Core.Interfaces;
using Mihaylov.Core.Managers;
using Mihaylov.Data.Models.Repositories;
using Ninject.Extensions.Logging;
using Telerik.JustMock;

namespace Mihaylov.Core.Tests.Managers
{
    [TestClass]
    public class CountriesManagerTests
    {
        private const int NUMBER_OF_COUNTRIES = 5;
        private const string NAME_TEMPLATE = "Country {0}";

        [TestMethod]
        public void ConstructiorProviderNullTest()
        {
            ILogger logger = Mock.Create<ILogger>();

            Assert.ThrowsException<ArgumentNullException>(() => new CountriesManager(null, logger));
        }

        [TestMethod]
        public void ConstructiorLoggerNullTest()
        {
            ICountriesProvider provider = Mock.Create<ICountriesProvider>();

            Assert.ThrowsException<ArgumentNullException>(() => new CountriesManager(provider, null));
        }

        [TestMethod]
        public void GeCountryByIdTest()
        {
            int id = 2;

            ILogger logger = Mock.Create<ILogger>();
            ICountriesProvider provider = GetCountriesProvider();
            ICountriesManager manager = new CountriesManager(provider, logger);
            
            Country result = manager.GetById(id);

            Assert.IsNotNull(result);
            Assert.AreEqual(id, result.Id);
            Assert.IsNotNull(result.Name);

            int records = ((CountriesManager)manager).NumberOfCountriesById;
            Assert.AreEqual(1, records);
        }

        [TestMethod]
        public void GetCountryByIdInvalidIdTest()
        {
            int id = NUMBER_OF_COUNTRIES + 1;

            ILogger logger = Mock.Create<ILogger>();
            ICountriesProvider provider = GetCountriesProvider();
            ICountriesManager manager = new CountriesManager(provider, logger);

            Country result = manager.GetById(id);

            Assert.IsNull(result);

            int records = ((CountriesManager)manager).NumberOfCountriesById;
            Assert.AreEqual(0, records);
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
        public void GetCountryByNameTest()
        {
            string name = string.Format(NAME_TEMPLATE, 3);

            ILogger logger = Mock.Create<ILogger>();
            ICountriesProvider provider = GetCountriesProvider();
            ICountriesManager manager = new CountriesManager(provider, logger);

            Country result = manager.GetByName(name);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Id > 0);
            Assert.AreEqual(name, result.Name);

            int records = ((CountriesManager)manager).NumberOfCountriesByName;
            Assert.AreEqual(1, records);
        }

        [TestMethod]
        public void GetCountryByNameUpperCaseTest()
        {
            string validName = string.Format(NAME_TEMPLATE, 3);
            string name = validName.ToUpper();

            ILogger logger = Mock.Create<ILogger>();
            ICountriesProvider provider = GetCountriesProvider();
            ICountriesManager manager = new CountriesManager(provider, logger);

            Country result = manager.GetByName(name);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Id > 0);
            Assert.AreEqual(validName, result.Name);

            int records = ((CountriesManager)manager).NumberOfCountriesByName;
            Assert.AreEqual(1, records);
        }

        [TestMethod]
        public void GetCountryByNameLowerCaseTest()
        {
            string validName = string.Format(NAME_TEMPLATE, 3);
            string name = validName.ToLower();

            ILogger logger = Mock.Create<ILogger>();
            ICountriesProvider provider = GetCountriesProvider();
            ICountriesManager manager = new CountriesManager(provider, logger);

            Country result = manager.GetByName(name);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Id > 0);
            Assert.AreEqual(validName, result.Name);

            int records = ((CountriesManager)manager).NumberOfCountriesByName;
            Assert.AreEqual(1, records);
        }

        [TestMethod]
        public void GetCountryByNameWithWhiteSpacesTest()
        {
            string validName = string.Format(NAME_TEMPLATE, 3);
            string name = string.Format("  {0}  ", validName);

            ILogger logger = Mock.Create<ILogger>();
            ICountriesProvider provider = GetCountriesProvider();
            ICountriesManager manager = new CountriesManager(provider, logger);

            Country result = manager.GetByName(name);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Id > 0);
            Assert.AreEqual(validName, result.Name);

            int records = ((CountriesManager)manager).NumberOfCountriesByName;
            Assert.AreEqual(1, records);
        }

        [TestMethod]
        public void GetCountryByNameInvalidNameTest()
        {
            string name = "abc";

            ILogger logger = Mock.Create<ILogger>();
            ICountriesProvider provider = GetCountriesProvider();
            ICountriesManager manager = new CountriesManager(provider, logger);

            Country result = manager.GetByName(name);

            Assert.IsNull(result);

            int records = ((CountriesManager)manager).NumberOfCountriesByName;
            Assert.AreEqual(0, records);
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
