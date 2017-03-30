using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mihaylov.Core.Interfaces;
using Mihaylov.Core.Managers;
using Mihaylov.Data.Models.Repositories;
using Ninject.Extensions.Logging;
using Telerik.JustMock;

namespace Mihaylov.Tests.Core
{
    [TestClass]
    public class CountriesManagerTests
    {
        private const int NUMBER_OF_COUNTRIES = 5;
        private const string NAME_TEMPLATE = "Country {0}";

        [TestMethod]
        public void GeCountryByIdTest()
        {
            int id = 2;

            ICountriesManager manager = GetCountriesManagerMock();
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

            ICountriesManager manager = GetCountriesManagerMock();
            Country result = manager.GetById(id);

            Assert.IsNull(result);

            int records = ((CountriesManager)manager).NumberOfCountriesById;
            Assert.AreEqual(0, records);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GetCountryByIdZeroIdTest()
        {
            int id = 0;

            ICountriesManager manager = GetCountriesManagerMock();
            Country result = manager.GetById(id);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GetCountryByIdNegativeIdTest()
        {
            int id = -3;

            ICountriesManager manager = GetCountriesManagerMock();
            Country result = manager.GetById(id);
        }

        [TestMethod]
        public void GetCountryByNameTest()
        {
            string name = string.Format(NAME_TEMPLATE, 3);

            ICountriesManager manager = GetCountriesManagerMock();
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

            ICountriesManager manager = GetCountriesManagerMock();
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

            ICountriesManager manager = GetCountriesManagerMock();
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

            ICountriesManager manager = GetCountriesManagerMock();
            Country result = manager.GetByName(name);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Id > 0);
            Assert.AreEqual(validName, result.Name);

            int records = ((CountriesManager)manager).NumberOfCountriesByName;
            Assert.AreEqual(1, records);
        }

        [TestMethod]
        public void GetCountryByNameInvaliNameTest()
        {
            string name = "abc";

            ICountriesManager manager = GetCountriesManagerMock();
            Country result = manager.GetByName(name);

            Assert.IsNull(result);

            int records = ((CountriesManager)manager).NumberOfCountriesByName;
            Assert.AreEqual(0, records);
        }

        private ICountriesManager GetCountriesManagerMock()
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

            var logger = Mock.Create<ILogger>();

            var manager = new CountriesManager(provider, logger);
            return manager;
        }
    }
}
