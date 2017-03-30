using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mihaylov.Core.Interfaces;
using Mihaylov.Core.Managers;
using Mihaylov.Data.Models.Repositories;
using Telerik.JustMock;

namespace Mihaylov.Tests.Core
{
    [TestClass]
    public class AnswerTypesManagerTests
    {
        private const int NUMBER_OF_ANSWER_TYPES = 5;
        private const string NAME_TEMPLATE = "Name {0}";

        [TestMethod]
        public void GetAllAnswersTypesTest()
        {
            IAnswerTypesManager manager = GetAnswerTypeManagerMock();
            IEnumerable<AnswerType> result = manager.GetAllAnswerTypes();

            Assert.IsNotNull(result);
            Assert.AreEqual(NUMBER_OF_ANSWER_TYPES, result.Count());
        }

        [TestMethod]
        public void GetAnswersTypeByIdTest()
        {
            int id = 2;

            IAnswerTypesManager manager = GetAnswerTypeManagerMock();
            AnswerType result = manager.GetById(id);

            Assert.IsNotNull(result);
            Assert.AreEqual(id, result.Id);
            Assert.IsNotNull(result.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void GetAnswersTypeByIdInvalidIdTest()
        {
            int id = NUMBER_OF_ANSWER_TYPES + 1;

            IAnswerTypesManager manager = GetAnswerTypeManagerMock();
            AnswerType result = manager.GetById(id);
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void GetAnswersTypeByIdZeroIdTest()
        {
            int id = 0;

            IAnswerTypesManager manager = GetAnswerTypeManagerMock();
            AnswerType result = manager.GetById(id);
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void GetAnswersTypeByIdNegativeIdTest()
        {
            int id = -3;

            IAnswerTypesManager manager = GetAnswerTypeManagerMock();
            AnswerType result = manager.GetById(id);
        }

        [TestMethod]
        public void GetAnswersTypeByNameTest()
        {
            string name = string.Format(NAME_TEMPLATE, 3);

            IAnswerTypesManager manager = GetAnswerTypeManagerMock();
            AnswerType result = manager.GetByName(name);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Id > 0);
            Assert.AreEqual(name, result.Name);
        }

        [TestMethod]
        public void GetAnswersTypeByNameWithWhiteSpacesTest()
        {
            string validName = string.Format(NAME_TEMPLATE, 3);
            string name = string.Format("  {0}  ", validName);

            IAnswerTypesManager manager = GetAnswerTypeManagerMock();
            AnswerType result = manager.GetByName(name);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Id > 0);
            Assert.AreEqual(validName, result.Name);
        }

        [TestMethod]
        public void GetAnswersTypeByNameUpperTest()
        {
            string validName = string.Format(NAME_TEMPLATE, 3);
            string name = validName.ToUpper();

            IAnswerTypesManager manager = GetAnswerTypeManagerMock();
            AnswerType result = manager.GetByName(name);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Id > 0);
            Assert.AreEqual(validName, result.Name);
        }

        [TestMethod]
        public void GetAnswersTypeByNameLowerTest()
        {
            string validName = string.Format(NAME_TEMPLATE, 3);
            string name = validName.ToLower();

            IAnswerTypesManager manager = GetAnswerTypeManagerMock();
            AnswerType result = manager.GetByName(name);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Id > 0);
            Assert.AreEqual(validName, result.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void GetAnswersTypeByNameInvaliNameTest()
        {
            string name = "abc";

            IAnswerTypesManager manager = GetAnswerTypeManagerMock();
            AnswerType result = manager.GetByName(name);
        }

        private IAnswerTypesManager GetAnswerTypeManagerMock()
        {
            var provider = Mock.Create<IAnswerTypesProvider>();
            Mock.Arrange(() => provider.GetAllAnswerTypes()).Returns(() =>
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
            });

            var manager = new AnswerTypesManager(provider);
            return manager;
        }
    }
}
