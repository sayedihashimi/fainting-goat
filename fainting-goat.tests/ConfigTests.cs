namespace fainting.goat.tests {
    using fainting.goat.common;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System.Collections.Generic;
    using System.Configuration;

    public class ConfigTests {
        [TestClass]
        public class TheGetConfigValueMethod {
            [TestMethod]
            public void ReturnsTheRequestedValue() {
                // <add key="sampleSetting" value="default-value"/>
                IConfig config = new Config();

                string settingName = "sampleSetting";
                string expectedResult = "default-value";
                string actualResult = config.GetConfigValue(settingName);

                Assert.AreEqual(expectedResult, actualResult);
            }

            [TestMethod]
            public void ReturnsNullWhenNotFoundAndNotRequired() {
                IConfig config = new Config();
                string result = config.GetConfigValue("doesnt-exist", isRequired: false);

                Assert.IsNull(result);
            }

            [TestMethod]
            [ExpectedException(typeof(ConfigurationErrorsException))]
            public void ThrowsWhenNotFoundAndRequired() {
                IConfig config = new Config();
                string result = config.GetConfigValue("doesnt-exist", isRequired: true);
            }

            [TestMethod]
            public void ReturnsTheDefaultValueWhenConfigValueNotFound() {
                IConfig config = new Config();
                string expectedResult = "default-value";
                string actualResult = config.GetConfigValue("doesnt-exist", false, defaultValue: expectedResult);

                Assert.AreEqual(expectedResult, actualResult);
            }
        }
        [TestClass]
        public class TheGetListMethod {
            [TestMethod]
            public void ReturnsTheGivenList() {
                IConfig config = new Config();
                IList<string> actualResult = config.GetList("sampleList");

                IList<string> expectedResult = new List<string> { "one", "two", "three", "four" };

                CustomAssert.AreEqual<string>(expectedResult, actualResult, (x, y) => { Assert.AreEqual(x, y); });
            }

            [TestMethod]
            [ExpectedException(typeof(ConfigurationErrorsException))]
            public void ThrowsWhenTheValueIsNotInConfigAndRequired() {
                IConfig config = new Config();
                IList<string> actualResult = config.GetList("doesnt-exist", isRequired: true);
            }

            [TestMethod]
            public void ReturnsNullWhenNotInConfigAndNotRequired() {
                IConfig config = new Config();
                IList<string> actualResult = config.GetList("doesnt-exist", isRequired: false);

                Assert.IsNull(actualResult);
            }

            [TestMethod]
            public void ReturnsTheExpectedResultWhenACustomDelimterIsUsed() {
                IConfig config = new Config();
                IList<string> actualResult = config.GetList("sampleList-customDelim",delimiter:'|');

                IList<string> expectedResult = new List<string> { "one", "two", "three", "four" };

                CustomAssert.AreEqual<string>(expectedResult, actualResult, (x, y) => { Assert.AreEqual(x, y); });
            }

            [TestMethod]
            public void ReturnsTheDefaultValueWhenConfigValueNotFound() {
                IConfig config = new Config();
                IList<string> expectedResult = new List<string> { "default", "value", "here" };
                IList<string> actualResult = config.GetList("doesnt-exist", defaultValue: expectedResult);

                Assert.AreEqual(expectedResult, actualResult);
            }
        }

        [TestClass]
        public class TheGetContentProviderTypeFromConfigMethod {
            [TestMethod]
            public void ReturnsGitWhenConfigHasGitUrl() {
                var mockConfig = new Mock<IConfig>();

                // for moq you have to specify default params http://code.google.com/p/moq/issues/detail?id=284
                mockConfig.Setup(config => config
                    .GetConfigValue(It.IsAny<string>()))
                    .Returns<string>(s => {
                        string retValue = null;

                        if (string.Compare(s, CommonConsts.AppSettings.GitUri, System.StringComparison.OrdinalIgnoreCase) == 0) {
                            retValue = @"https://github.com/sayedihashimi/publish-samples.git";
                        }

                        return retValue;
                    });

                MdContentProviderType mdType = Config.GetContentProviderTypeFromConfig(mockConfig.Object);

                Assert.AreEqual(MdContentProviderType.Git, mdType);
            }

            [TestMethod]
            public void ReturnsFileSystemWhenConfigDoesNotHaveGirUrl() {
                var mockConfig = new Mock<IConfig>();

                mockConfig.Setup(config => config
                    .GetConfigValue(It.IsAny<string>()))
                    .Returns<string>(s => {
                        return null;
                    });

                MdContentProviderType mdType = Config.GetContentProviderTypeFromConfig(mockConfig.Object);

                Assert.AreEqual(MdContentProviderType.FileSystem, mdType);
            }
        }
    }
}
