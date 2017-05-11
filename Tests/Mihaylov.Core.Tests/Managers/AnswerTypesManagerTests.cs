using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mihaylov.Core.Interfaces;
using Mihaylov.Core.Managers;
using Mihaylov.Core.Tests.Managers.Fakes;
using Mihaylov.Data.Models.Repositories;
using Telerik.JustMock;

namespace Mihaylov.Core.Tests.Managers
{
    [TestClass]
    public class AnswerTypesManagerTests
    {
        private const int NUMBER_OF_ANSWER_TYPES = 5;
        private const string NAME_TEMPLATE = "Name {0}";
        private const string VALID_NAME = "Name 3";

        [TestMethod]
        public void ConstructiorProviderNullTest()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new AnswerTypesManager(null));
        }

        [TestMethod]
        public void ConstructiorProviderIsSetProperly()
        {
            IAnswerTypesProvider providerMock = this.GetAnswerTypeProviderMock();
            var manager = new AnswerTypesManagerFake(providerMock);

            Assert.AreSame(providerMock, manager.ExposedProvider);
        }

        [TestMethod]
        public void ConstructiorDicionaryByIdInitialized()
        {
            IAnswerTypesProvider providerMock = this.GetAnswerTypeProviderMock();
            var manager = new AnswerTypesManagerFake(providerMock);

            Assert.IsNotNull(manager.ExposedDictionaryById);
        }

        [TestMethod]
        public void ConstructiorDicionaryByNameInitialized()
        {
            IAnswerTypesProvider providerMock = this.GetAnswerTypeProviderMock();
            var manager = new AnswerTypesManagerFake(providerMock);

            Assert.IsNotNull(manager.ExposedDictionaryByName);
        }

        [TestMethod]
        public void ConstructiorDicionaryByIdFilled()
        {
            IAnswerTypesProvider providerMock = this.GetAnswerTypeProviderMock();
            var manager = new AnswerTypesManagerFake(providerMock);

          //  Mock.Assert(() => providerMock.GetAllAnswerTypes(), Occurs.Once());
            Assert.AreEqual(NUMBER_OF_ANSWER_TYPES, manager.ExposedDictionaryById.Keys.Count);
        }

        [TestMethod]
        public void ConstructiorDicionaryByNameFilled()
        {
            IAnswerTypesProvider providerMock = this.GetAnswerTypeProviderMock();
            var manager = new AnswerTypesManagerFake(providerMock);

           // Mock.Assert(() => providerMock.GetAllAnswerTypes(), Occurs.Once());
            Assert.AreEqual(NUMBER_OF_ANSWER_TYPES, manager.ExposedDictionaryByName.Keys.Count);
        }

        [TestMethod]
        public void GetAllAnswersTypesTest()
        {
            IAnswerTypesProvider providerMock = this.GetAnswerTypeProviderMock();
            IAnswerTypesManager manager = new AnswerTypesManager(providerMock);

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
            IAnswerTypesProvider providerMock = this.GetAnswerTypeProviderMock();
            IAnswerTypesManager manager = new AnswerTypesManager(providerMock);

            AnswerType result = manager.GetById(id);

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
            IAnswerTypesProvider providerMock = this.GetAnswerTypeProviderMock();
            IAnswerTypesManager manager = new AnswerTypesManager(providerMock);

            Assert.ThrowsException<ApplicationException>(() => manager.GetById(id));
        }

        [TestMethod]
        public void GetAnswersTypeByNameInvaliNameTest()
        {
            string name = "abc";

            IAnswerTypesProvider providerMock = this.GetAnswerTypeProviderMock();
            IAnswerTypesManager manager = new AnswerTypesManager(providerMock);

            Assert.ThrowsException<ApplicationException>(() => manager.GetByName(name));
        }

        [TestMethod]
        [DataRow(VALID_NAME, VALID_NAME, DisplayName = "Valid name")]
        [DataRow(VALID_NAME, "  " + VALID_NAME + "   ", DisplayName = "Name with white spaces")]
       // [DataRow(VALID_NAME, VALID_NAME.ToUpper(), DisplayName = "Uppercase Name")]
       // [DataRow(VALID_NAME, VALID_NAME.ToLower(), DisplayName = "Lowercase Name")]
        public void GetAnswersTypeByNameTest(string name, string inputName)
        {
            IAnswerTypesProvider providerMock = this.GetAnswerTypeProviderMock();
            IAnswerTypesManager manager = new AnswerTypesManager(providerMock);

            AnswerType result = manager.GetByName(inputName);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Id > 0);
            Assert.AreEqual(name, result.Name);
        }

        [TestMethod]
        public void GetAnswersTypeByNameUpperTest()
        {
            string name = VALID_NAME.ToUpper();

            IAnswerTypesProvider providerMock = this.GetAnswerTypeProviderMock();
            IAnswerTypesManager manager = new AnswerTypesManager(providerMock);

            AnswerType result = manager.GetByName(name);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Id > 0);
            Assert.AreEqual(VALID_NAME, result.Name);
        }

        [TestMethod]
        public void GetAnswersTypeByNameLowerTest()
        {
            string name = VALID_NAME.ToLower();

            IAnswerTypesProvider providerMock = this.GetAnswerTypeProviderMock();
            IAnswerTypesManager manager = new AnswerTypesManager(providerMock);

            AnswerType result = manager.GetByName(name);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Id > 0);
            Assert.AreEqual(VALID_NAME, result.Name);
        }

        private IAnswerTypesProvider GetAnswerTypeProviderMock()
        {
            var provider = Mock.Create<IAnswerTypesProvider>();

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

            return provider;
        }
    }
}
