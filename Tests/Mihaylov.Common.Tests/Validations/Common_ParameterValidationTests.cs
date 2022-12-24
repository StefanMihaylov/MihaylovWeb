//using System;
//using System.Collections.Generic;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Mihaylov.Common.Validations;

//namespace Mihaylov.Common.Tests.Validations
//{
//    [TestClass]
//    public class Common_ParameterValidationTests
//    {
//        [TestMethod]
//        public void TestNullValidationPass()
//        {
//            var testObject = new List<string>();
//            ParameterValidation.IsNotNull(testObject, nameof(testObject));            
//        }

//        [TestMethod]
//        public void TestNullValidationFail()
//        {
//            List<int> testObject = null;
//            Assert.ThrowsException<ArgumentNullException>(() => ParameterValidation.IsNotNull(testObject, nameof(testObject)));
//        }

//        [TestMethod]
//        public void TestEmpyStringValidationPass()
//        {
//            string testObject = "abc";
//            ParameterValidation.IsNotEmptyString(testObject, nameof(testObject));
//        }

//        [TestMethod]
//        [DataRow(null)]
//        [DataRow("")]
//        [DataRow(" ")]
//        [DataRow("\n")]
//        public void TestEmptyStringValidationFail(string testObject)
//        {
//            Assert.ThrowsException<ArgumentException>(() => ParameterValidation.IsNotEmptyString(testObject, nameof(testObject)));
//        }
//    }
//}
