namespace fainting.goat.tests {
    using fainting.goat.common;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    public class FaintingGoatTests {
        [TestClass]
        public class TheGetContentProviderMethod {
            [TestMethod]
            public void ReturnsGitProviderWhenConfigHasGitUri() {

                var mockConfig = new Mock<IConfig>();

                mockConfig.Setup(config => config
                    .GetConfigValue(It.IsAny<string>()))
                    .Returns<string>(key => {
                        string retValue = null;
                        if (string.Compare(key, CommonConsts.AppSettings.GitUri, StringComparison.OrdinalIgnoreCase) == 0) {
                            retValue = @"https://github.com/sayedihashimi/publish-samples.git";
                        }
                        return retValue;
                    });

                string folderPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
                FaintingGoat goat = new FaintingGoat(mockConfig.Object, folderPath);
                var actualProvider = goat.GetContentProvider();

                Assert.AreEqual(MdContentProviderType.Git, actualProvider.ProviderType);
            }

            [TestMethod]
            public void ReturnsFileSystemIfConfigDoesNotHaveGitUri() {
                var mockConfig = new Mock<IConfig>();

                mockConfig.Setup(config => config
                    .GetConfigValue(It.IsAny<string>()))
                    .Returns<string>(key => {
                        return null;
                    });

                string folderPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
                FaintingGoat goat = new FaintingGoat(mockConfig.Object, folderPath);
                var actualProvider = goat.GetContentProvider();

                Assert.AreEqual(MdContentProviderType.FileSystem, actualProvider.ProviderType);
            }
        }
    }
}
