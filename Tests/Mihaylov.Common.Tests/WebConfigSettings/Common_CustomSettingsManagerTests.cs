using System;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mihaylov.Common.WebConfigSettings;
using Mihaylov.Common.WebConfigSettings.Interfaces;
using Mihaylov.Common.WebConfigSettings.Models;
using Telerik.JustMock;

namespace Mihaylov.Common.Tests.WebConfigSettings
{
    [TestClass]
    public class Common_CustomSettingsManagerTests
    {
        public const string ENVIRONMENT = "DEV";
        public const string LOGGER_PATH = @"C:\Logs";
        public const string SETTINGS_ONE_KEY = "abc";
        public const string SETTINGS_ONE_VALUE = "123";
        public const string SETTINGS_TWO_KEY = "987";
        public const string SETTINGS_TWO_VALUE = "ZYX";
        public const string SETTINGS_THREE_KEY = "PQR";
        public const string SETTINGS_THREE_VALUE = "stu";

        [TestMethod]
        public void ConstructorValitionTest()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new CustomSettingsManager(null));
        }

        [TestMethod]
        public void GetSettingsFromProviderOnlyOnceTest()
        {
            ICustomSettingsProvider provider = GetCustomSettingsPrroviderMock();
            ICustomSettingsManager manager = new CustomSettingsManager(provider);

            CustomSettingsModel settings = manager.Settings;
            Mock.Assert(() => provider.GetAllSettingByEnvironment(), Occurs.Once());

            CustomSettingsModel settingsSecond = manager.Settings;
            Mock.Assert(() => provider.GetAllSettingByEnvironment(), Occurs.Once());
        }

        [TestMethod]
        public void GetSettingsTest()
        {
            ICustomSettingsProvider provider = GetCustomSettingsPrroviderMock();
            ICustomSettingsManager manager = new CustomSettingsManager(provider);

            CustomSettingsModel settings = manager.Settings;

            Assert.IsNotNull(settings);
            Assert.AreEqual(ENVIRONMENT, settings.Environment);
            Assert.AreEqual(LOGGER_PATH, settings.LoggerPath);
            Assert.IsNotNull(settings.Settings);
            Assert.IsTrue(settings.Settings.ContainsKey(SETTINGS_ONE_KEY));
            Assert.IsTrue(settings.Settings.ContainsKey(SETTINGS_TWO_KEY));
            Assert.AreEqual(SETTINGS_ONE_VALUE, settings.Settings[SETTINGS_ONE_KEY]);
            Assert.AreEqual(SETTINGS_TWO_VALUE, settings.Settings[SETTINGS_TWO_KEY]);
        }

        [TestMethod]
        [DataRow(SETTINGS_ONE_KEY, SETTINGS_ONE_VALUE)]
        [DataRow(SETTINGS_TWO_KEY, SETTINGS_TWO_VALUE)]
        public void GetSettingByKeyTest(string key, string expectedValue)
        {
            ICustomSettingsProvider provider = GetCustomSettingsPrroviderMock();
            ICustomSettingsManager manager = new CustomSettingsManager(provider);

            var value = manager.GetSettingByName(key);
            Assert.AreEqual(expectedValue, value);
        }

        [TestMethod]
        public void GetSettingByKeyInvalidKey()
        {
            string key = SETTINGS_ONE_KEY + "1";

            ICustomSettingsProvider provider = GetCustomSettingsPrroviderMock();
            ICustomSettingsManager manager = new CustomSettingsManager(provider);

            Assert.ThrowsException<ConfigurationErrorsException>(() => manager.GetSettingByName(key));
        }

        [TestMethod]
        public void GetSettingByKeyUpperCaseTest()
        {
            string key = SETTINGS_ONE_KEY.ToUpper();
            string expectedValue = SETTINGS_ONE_VALUE;

            ICustomSettingsProvider provider = GetCustomSettingsPrroviderMock();
            ICustomSettingsManager manager = new CustomSettingsManager(provider);

            var value = manager.GetSettingByName(key);
            Assert.AreEqual(expectedValue, value);
        }

        [TestMethod]
        public void GetSettingByKeyLowerCaseTest()
        {
            string key = SETTINGS_THREE_KEY.ToLower();
            string expectedValue = SETTINGS_THREE_VALUE;

            ICustomSettingsProvider provider = GetCustomSettingsPrroviderMock();
            ICustomSettingsManager manager = new CustomSettingsManager(provider);

            var value = manager.GetSettingByName(key);
            Assert.AreEqual(expectedValue, value);
        }

        [TestMethod]
        public void GetSettingByKeyAndCastTest()
        {
            ICustomSettingsProvider provider = GetCustomSettingsPrroviderMock();
            ICustomSettingsManager manager = new CustomSettingsManager(provider);

            var value = manager.GetSettingByName<int>(SETTINGS_ONE_KEY);
            Assert.AreEqual(int.Parse(SETTINGS_ONE_VALUE), value);
        }

        [TestMethod]
        public void GetSettingByKeyAndCannotCastTest()
        {
            ICustomSettingsProvider provider = GetCustomSettingsPrroviderMock();
            ICustomSettingsManager manager = new CustomSettingsManager(provider);

            Assert.ThrowsException<Exception>(() => manager.GetSettingByName<int>(SETTINGS_TWO_KEY));
        }

        [TestMethod]
        public void GetSettingByKeyThrowExceptionWhenKeysAreDuplictedTest()
        {
            ICustomSettingsProvider provider = GetCustomSettingsPrroviderMock(true);
            ICustomSettingsManager manager = new CustomSettingsManager(provider);

            Assert.ThrowsException<ArgumentException>(() => manager.GetSettingByName(SETTINGS_TWO_KEY));
        }

        private ICustomSettingsProvider GetCustomSettingsPrroviderMock(bool addInvalidRow = false)
        {
            var provider = Mock.Create<ICustomSettingsProvider>();

            var settings = new Dictionary<string, string>()
            {
                { SETTINGS_ONE_KEY, SETTINGS_ONE_VALUE },
                { SETTINGS_TWO_KEY, SETTINGS_TWO_VALUE },
                { SETTINGS_THREE_KEY, SETTINGS_THREE_VALUE },
            };

            if (addInvalidRow)
            {
                settings.Add(SETTINGS_THREE_KEY.ToLower(), SETTINGS_THREE_VALUE);
            }

            Mock.Arrange(() => provider.GetAllSettingByEnvironment())
                .Returns(new CustomSettingsModel()
                {
                    Environment = ENVIRONMENT,
                    LoggerPath = LOGGER_PATH,
                    Settings = settings,
                });

            return provider;
        }
    }
}
