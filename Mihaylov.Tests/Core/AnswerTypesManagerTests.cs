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
        private const int NUMBER = 5;
        private const string NAME_TEMPLATE = "Type {0}";

        [TestMethod]
        public void GetAllAnswersTypesTest()
        {
            IAnswerTypesManager manager = GetAnswerTypeManagerMock();
            IEnumerable<AnswerType> result = manager.GetAllAnswerTypes();

            Assert.IsNotNull(result);
            Assert.AreEqual(NUMBER, result.Count());
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
            int id = NUMBER + 1;

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
            string name = $"  {validName}  ";

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
                for (int index = 1; index <= NUMBER; index++)
                {
                    result.Add(new AnswerType() { Id = index, Name = string.Format(NAME_TEMPLATE, index), Description = $"N/A {index}", IsAsked = (index % 2 == 0) });
                }

                return result;
            });

            var manager = new AnswerTypesManager(provider);
            return manager;
        }
    }
}
