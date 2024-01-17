using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mihaylov.Site.Core.Managers;
using Moq;
using System;

namespace Mihaylov.Core.Tests.Managers
{
    //[TestClass]
    public class Site_Core_PersonAdditionalInfoManagerTests
    {
        private const int NUMBER_OF_ANSWER_TYPES = 5;
        private const string NAME_TEMPLATE = "Name {0}";
        private const string VALID_NAME = "Name 3";

        private const int NUMBER_OF_COUNTRIES = 5;
        private const string COUNTRY_NAME_TEMPLATE = "Country {0}";
        private const string VALID_COUNTRY_NAME = "Country 3";

        [TestMethod]
        public void ConstructiorProviderNullMockedLoggerTest()
        {
            ILoggerFactory logger = GetLoggerMocked();
           // IMessageBus messageBus = GetMessageBusMocked();

            Assert.ThrowsException<ArgumentNullException>(() => new CollectionsManager(null, null, logger));
        }

        /*

        [TestMethod]
        public void ConstructiorLoggerNullTest()
        {
            IPersonAdditionalInfoProvider provider = Mock.Create<IPersonAdditionalInfoProvider>();
            IMessageBus messageBus = GetMessageBusMocked();

            Assert.ThrowsException<ArgumentNullException>(() => new PersonAdditionalInfoManager(provider, null, messageBus));
        }

        [TestMethod]
        public void ConstructiorMessageBusNullTest()
        {
            IPersonAdditionalInfoProvider provider = Mock.Create<IPersonAdditionalInfoProvider>();
            ILogger logger = GetLoggerMocked();

            Assert.ThrowsException<ArgumentNullException>(() => new PersonAdditionalInfoManager(provider, logger, null));
        }

        [TestMethod]
        public void ConstructiorProviderIsSetProperly()
        {
            ILogger logger = GetLoggerMocked();
            IPersonAdditionalInfoProvider providerMock = this.GetPersonAdditionalInfoProviderMock();
            IMessageBus messageBus = GetMessageBusMocked();
            var manager = new PersonAdditionalInfoManagerFake(providerMock, logger, messageBus);

            Assert.AreSame(providerMock, manager.ExposedProvider);
        }

        [TestMethod]
        public void ConstructiorLoggerIsSetProperly()
        {
            IPersonAdditionalInfoProvider providerMock = this.GetPersonAdditionalInfoProviderMock();
            ILogger logger = this.GetLoggerMocked();
            IMessageBus messageBus = GetMessageBusMocked();
            var manager = new PersonAdditionalInfoManagerFake(providerMock, logger, messageBus);

            Assert.AreSame(logger, manager.ExposedLogger);
        }

        [TestMethod]
        public void ConstructiorMEssageBusIsSetProperly()
        {
            IPersonAdditionalInfoProvider providerMock = this.GetPersonAdditionalInfoProviderMock();
            ILogger logger = this.GetLoggerMocked();
            IMessageBus messageBus = GetMessageBusMocked();
            var manager = new PersonAdditionalInfoManagerFake(providerMock, logger, messageBus);

            Assert.AreSame(messageBus, manager.ExposedMessageBus);
        }

        [TestMethod]
        public void ConstructiorDicionaryByIdInitialized()
        {
            IPersonAdditionalInfoProvider providerMock = this.GetPersonAdditionalInfoProviderMock();
            ILogger logger = GetLoggerMocked();
            IMessageBus messageBus = GetMessageBusMocked();
            var manager = new PersonAdditionalInfoManagerFake(providerMock, logger, messageBus);

            Assert.IsNotNull(manager.ExposedAnswerTypeDictionaryById);
        }

        [TestMethod]
        public void ConstructiorDicionaryByNameInitialized()
        {
            IPersonAdditionalInfoProvider providerMock = this.GetPersonAdditionalInfoProviderMock();
            ILogger logger = GetLoggerMocked();
            IMessageBus messageBus = GetMessageBusMocked();
            var manager = new PersonAdditionalInfoManagerFake(providerMock, logger, messageBus);

            Assert.IsNotNull(manager.ExposedAnswerTypeDictionaryByName);
        }

        [TestMethod]
        public void ConstructiorDicionaryByIdFilled()
        {
            IPersonAdditionalInfoProvider providerMock = this.GetPersonAdditionalInfoProviderMock();
            ILogger logger = GetLoggerMocked();
            IMessageBus messageBus = GetMessageBusMocked();
            var manager = new PersonAdditionalInfoManagerFake(providerMock, logger, messageBus);

          //  Mock.Assert(() => providerMock.GetAllAnswerTypes(), Occurs.Once());
            Assert.AreEqual(NUMBER_OF_ANSWER_TYPES, manager.ExposedAnswerTypeDictionaryById.Keys.Count);
        }

        [TestMethod]
        public void ConstructiorDicionaryByNameFilled()
        {
            IPersonAdditionalInfoProvider providerMock = this.GetPersonAdditionalInfoProviderMock();
            ILogger logger = GetLoggerMocked();
            IMessageBus messageBus = GetMessageBusMocked();
            var manager = new PersonAdditionalInfoManagerFake(providerMock, logger, messageBus);

           // Mock.Assert(() => providerMock.GetAllAnswerTypes(), Occurs.Once());
            Assert.AreEqual(NUMBER_OF_ANSWER_TYPES, manager.ExposedAnswerTypeDictionaryByName.Keys.Count);
        }

        [TestMethod]
        public void GetAllAnswersTypesTest()
        {
            IPersonAdditionalInfoProvider providerMock = this.GetPersonAdditionalInfoProviderMock();
            ILogger logger = GetLoggerMocked();
            IMessageBus messageBus = GetMessageBusMocked();
            var manager = new PersonAdditionalInfoManager(providerMock, logger, messageBus);

            IEnumerable<AnswerType> result = manager.GetAllAnswerTypes();

            Assert.IsNotNull(result);
            Assert.AreEqual(NUMBER_OF_ANSWER_TYPES, result.Count());
        }

        [TestMethod]
        [DataRow(1, DisplayName = "Reqular Id, start")]
        [DataRow(NUMBER_OF_ANSWER_TYPES / 2, DisplayName = "Reqular Id, middle")]
        [DataRow(NUMBER_OF_ANSWER_TYPES, DisplayName = "Reqular Id, end")]
        public void GetAnswersTypeByIdTest(int id)
        {
            IPersonAdditionalInfoProvider providerMock = this.GetPersonAdditionalInfoProviderMock();
            ILogger logger = GetLoggerMocked();
            IMessageBus messageBus = GetMessageBusMocked();
            var manager = new PersonAdditionalInfoManager(providerMock, logger, messageBus);

            AnswerType result = manager.GetAnswerTypeById(id);

            Assert.IsNotNull(result);
            Assert.AreEqual(id, result.Id);
            Assert.IsNotNull(result.Name);
        }

        [TestMethod]
        [DataRow(-3, DisplayName = "below the range, negative")]
        [DataRow(0, DisplayName = "below the range, zero")]
        [DataRow(NUMBER_OF_ANSWER_TYPES + 1, DisplayName = "above the range")]
        public void GetAnswersTypeByIdInvalidIdTest(int id)
        {
            IPersonAdditionalInfoProvider providerMock = this.GetPersonAdditionalInfoProviderMock();
            ILogger logger = this.GetLoggerMocked();
            IMessageBus messageBus = GetMessageBusMocked();
            var manager = new PersonAdditionalInfoManager(providerMock, logger, messageBus);

            Assert.ThrowsException<ApplicationException>(() => manager.GetAnswerTypeById(id));
        }

        [TestMethod]
        public void GetAnswersTypeByNameInvaliNameTest()
        {
            string name = "abc";

            IPersonAdditionalInfoProvider providerMock = this.GetPersonAdditionalInfoProviderMock();
            ILogger logger = this.GetLoggerMocked();
            IMessageBus messageBus = GetMessageBusMocked();
            var manager = new PersonAdditionalInfoManager(providerMock, logger, messageBus);

            Assert.ThrowsException<ApplicationException>(() => manager.GetAnswerTypeByName(name));
        }

        [TestMethod]
        [DataRow(VALID_NAME, VALID_NAME, DisplayName = "Valid name")]
        [DataRow(VALID_NAME, "  " + VALID_NAME + "   ", DisplayName = "Name with white spaces")]
       // [DataRow(VALID_NAME, VALID_NAME.ToUpper(), DisplayName = "Uppercase Name")]
       // [DataRow(VALID_NAME, VALID_NAME.ToLower(), DisplayName = "Lowercase Name")]
        public void GetAnswersTypeByNameTest(string name, string inputName)
        {
            IPersonAdditionalInfoProvider providerMock = this.GetPersonAdditionalInfoProviderMock();
            ILogger logger = this.GetLoggerMocked();
            IMessageBus messageBus = GetMessageBusMocked();
            var manager = new PersonAdditionalInfoManager(providerMock, logger, messageBus);

            AnswerType result = manager.GetAnswerTypeByName(inputName);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Id > 0);
            Assert.AreEqual(name, result.Name);
        }

        [TestMethod]
        public void GetAnswersTypeByNameUpperTest()
        {
            string name = VALID_NAME.ToUpper();

            IPersonAdditionalInfoProvider providerMock = this.GetPersonAdditionalInfoProviderMock();
            ILogger logger = this.GetLoggerMocked();
            IMessageBus messageBus = GetMessageBusMocked();
            var manager = new PersonAdditionalInfoManager(providerMock, logger, messageBus);

            AnswerType result = manager.GetAnswerTypeByName(name);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Id > 0);
            Assert.AreEqual(VALID_NAME, result.Name);
        }

        [TestMethod]
        public void GetAnswersTypeByNameLowerTest()
        {
            string name = VALID_NAME.ToLower();

            IPersonAdditionalInfoProvider providerMock = this.GetPersonAdditionalInfoProviderMock();
            ILogger logger = this.GetLoggerMocked();
            IMessageBus messageBus = GetMessageBusMocked();
            var manager = new PersonAdditionalInfoManager(providerMock, logger, messageBus);

            AnswerType result = manager.GetAnswerTypeByName(name);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Id > 0);
            Assert.AreEqual(VALID_NAME, result.Name);
        }

        [TestMethod]
        [DataRow(2, DisplayName = "Reqular Id, start")]
        [DataRow(NUMBER_OF_COUNTRIES / 2, DisplayName = "Reqular Id, middle")]
        [DataRow(NUMBER_OF_COUNTRIES, DisplayName = "Reqular Id, end")]
        public void GeCountryByIdTest(int id)
        {
            IPersonAdditionalInfoProvider providerMock = this.GetPersonAdditionalInfoProviderMock();
            ILogger logger = this.GetLoggerMocked();
            IMessageBus messageBus = GetMessageBusMocked();
            var manager = new PersonAdditionalInfoManagerFake(providerMock, logger, messageBus);

            Country result = manager.GetCountryById(id);

            Assert.IsNotNull(result);
            Assert.AreEqual(id, result.Id);
            Assert.IsNotNull(result.Name);

            Assert.AreEqual(1, manager.ExposedCountryDictionaryById.Count);
        }

        [TestMethod]
        [DataRow(NUMBER_OF_COUNTRIES + 1, DisplayName = "Id, out of range")]
        public void GetCountryByIdInvalidIdTest(int id)
        {
            IPersonAdditionalInfoProvider providerMock = this.GetPersonAdditionalInfoProviderMock();
            ILogger logger = this.GetLoggerMocked();
            IMessageBus messageBus = GetMessageBusMocked();
            var manager = new PersonAdditionalInfoManagerFake(providerMock, logger, messageBus);

            Country result = manager.GetCountryById(id);

            Assert.IsNull(result);

            Assert.AreEqual(0, manager.ExposedCountryDictionaryById.Count);
        }

        [TestMethod]
        [DataRow(0, DisplayName = "Zero Id")]
        [DataRow(-3, DisplayName = "Negative Id")]
        public void GetCountryByIdIncorrectTest(int id)
        {
            IPersonAdditionalInfoProvider providerMock = this.GetPersonAdditionalInfoProviderMock();
            ILogger logger = this.GetLoggerMocked();
            IMessageBus messageBus = GetMessageBusMocked();
            var manager = new PersonAdditionalInfoManager(providerMock, logger, messageBus);

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => manager.GetCountryById(id));
        }

        [TestMethod]
        [DataRow(VALID_COUNTRY_NAME, VALID_COUNTRY_NAME, DisplayName = "Valid name")]
        [DataRow(VALID_COUNTRY_NAME, "  " + VALID_COUNTRY_NAME + "   ", DisplayName = "Name with white spaces")]
        public void GetCountryByNameTest(string name, string inputName)
        {
            IPersonAdditionalInfoProvider providerMock = this.GetPersonAdditionalInfoProviderMock();
            ILogger logger = this.GetLoggerMocked();
            IMessageBus messageBus = GetMessageBusMocked();
            var manager = new PersonAdditionalInfoManagerFake(providerMock, logger, messageBus);

            Country result = manager.GetCountryByName(inputName);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Id > 0);
            Assert.AreEqual(name, result.Name);

            Assert.AreEqual(1, manager.ExposedCountryDictionaryByName.Count);
        }

        [TestMethod]
        public void GetCountryByNameUpperCaseTest()
        {
            string validName = string.Format(COUNTRY_NAME_TEMPLATE, 3);
            string name = validName.ToUpper();

            IPersonAdditionalInfoProvider providerMock = this.GetPersonAdditionalInfoProviderMock();
            ILogger logger = this.GetLoggerMocked();
            IMessageBus messageBus = GetMessageBusMocked();
            var manager = new PersonAdditionalInfoManagerFake(providerMock, logger, messageBus);

            Country result = manager.GetCountryByName(name);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Id > 0);
            Assert.AreEqual(validName, result.Name);

            Assert.AreEqual(1, manager.ExposedCountryDictionaryByName.Count);
        }

        [TestMethod]
        public void GetCountryByNameLowerCaseTest()
        {
            string validName = string.Format(COUNTRY_NAME_TEMPLATE, 3);
            string name = validName.ToLower();

            IPersonAdditionalInfoProvider providerMock = this.GetPersonAdditionalInfoProviderMock();
            ILogger logger = this.GetLoggerMocked();
            IMessageBus messageBus = GetMessageBusMocked();
            var manager = new PersonAdditionalInfoManagerFake(providerMock, logger, messageBus);

            Country result = manager.GetCountryByName(name);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Id > 0);
            Assert.AreEqual(validName, result.Name);

            Assert.AreEqual(1, manager.ExposedCountryDictionaryByName.Count);
        }

        [TestMethod]
        public void GetCountryByNameInvalidNameTest()
        {
            string name = "abc";

            IPersonAdditionalInfoProvider providerMock = this.GetPersonAdditionalInfoProviderMock();
            ILogger logger = this.GetLoggerMocked();
            IMessageBus messageBus = GetMessageBusMocked();
            var manager = new PersonAdditionalInfoManagerFake(providerMock, logger, messageBus);

            Country result = manager.GetCountryByName(name);

            Assert.IsNull(result);

            Assert.AreEqual(0, manager.ExposedCountryDictionaryByName.Count);
        }

        [TestMethod]
        public void RequestByNameIsLoggedTest()
        {
            string name = VALID_COUNTRY_NAME;

            IPersonAdditionalInfoProvider providerMock = this.GetPersonAdditionalInfoProviderMock();
            ILogger logger = this.GetLoggerMocked();
            IMessageBus messageBus = GetMessageBusMocked();
            var manager = new PersonAdditionalInfoManagerFake(providerMock, logger, messageBus);

            Country result = manager.GetCountryByName(name);

            Mock.Assert(() => logger.Debug(Arg.AnyString), Occurs.AtLeastOnce());
        }

        [TestMethod]
        public void RequestByNameCallProviderByNameTest()
        {
            string name = VALID_COUNTRY_NAME;

            IPersonAdditionalInfoProvider providerMock = this.GetPersonAdditionalInfoProviderMock();
            ILogger logger = this.GetLoggerMocked();
            IMessageBus messageBus = GetMessageBusMocked();
            var manager = new PersonAdditionalInfoManagerFake(providerMock, logger, messageBus);

            Country result = manager.GetCountryByName(name);

            Mock.Assert(() => providerMock.GetCountryByName(Arg.AnyString), Occurs.Once());
        }

        [TestMethod]
        public void RequestByIdCallProviderByIdTest()
        {
            int id = 2;

            IPersonAdditionalInfoProvider providerMock = this.GetPersonAdditionalInfoProviderMock();
            ILogger logger = this.GetLoggerMocked();
            IMessageBus messageBus = GetMessageBusMocked();
            var manager = new PersonAdditionalInfoManagerFake(providerMock, logger, messageBus);

            Country result = manager.GetCountryById(id);

            Mock.Assert(() => providerMock.GetCountryById(Arg.AnyInt), Occurs.Once());
        }


        private IPersonAdditionalInfoProvider GetPersonAdditionalInfoProviderMock()
        {
            var provider = Mock.Create<IPersonAdditionalInfoProvider>();

            Mock.Arrange(() => provider.GetAllAnswerTypes())
                .Returns(() =>
                {
                    var result = new List<AnswerType>();
                    for (int index = 1; index <= NUMBER_OF_ANSWER_TYPES; index++)
                    {
                        result.Add(new AnswerType()
                        {
                            Id = index,
                            Name = string.Format(NAME_TEMPLATE, index),
                            Description = string.Format("N/A {0}", index),
                            IsAsked = (index % 2 == 0)
                        });
                    }
                
                    return result;
                })
                .MustBeCalled();


            var countries = new List<Country>();
            for (int index = 1; index <= NUMBER_OF_COUNTRIES; index++)
            {
                countries.Add(new Country()
                {
                    Id = index,
                    Name = string.Format(COUNTRY_NAME_TEMPLATE, index),
                    Description = string.Format("N/A {0}", index)
                });
            }

            Mock.Arrange(() => provider.GetCountryById(Arg.AnyInt)).Returns<int>((id) =>
            {
                Country result = countries.FirstOrDefault(c => c.Id == id);
                if (result == null)
                {
                    throw new ApplicationException();
                }

                return result;
            });

            Mock.Arrange(() => provider.GetCountryByName(Arg.AnyString)).Returns<string>((name) =>
            {
                Country result = countries.FirstOrDefault(c => c.Name.ToLower() == name.ToLower());
                if (result == null)
                {
                    throw new ApplicationException();
                }

                return result;
            });

            Mock.Arrange(() => provider.GetAllCountries()).Returns(countries);

            return provider;
        }

     */

        private ILoggerFactory GetLoggerMocked()
        {
            return NullLoggerFactory.Instance;
        }

        //private IMessageBus GetMessageBusMocked()
        //{
        //    return new Mock<IMessageBus>().Object;
        //}
       
    }
}
