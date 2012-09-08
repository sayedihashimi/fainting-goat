namespace fainting.goat.tests {
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using fainting.goat.tests.Helpers;
    using System.Text;
    using fainting.goat.common;

    [TestClass]
    public class FileContentRepositoryTests {

        [TestClass]
        public class TheGetContentForMethod : BaseTest {
            [TestMethod]
            public void ReturnsTheFileContents() {
                // write a dummy file to disk and have the FileRepo read it back                               
                StringBuilder fileContents = new StringBuilder();
                int numLines = RandomDataHelper.Instance.Primitives.GetRandomInt(20);
                for (int i = 0; i < numLines; i++) {
                    fileContents.AppendLine(RandomDataHelper.Instance.Primitives.GetRandomString(200));
                }

                string fileContentsExpected = fileContents.ToString();
                string tempFile = this.WriteTextToTempFile(fileContentsExpected);

                IContentRepository fileRepo = new FileContentRepository(new PathHelper(new Config()));
                string fileContentsActual = fileRepo.GetContentFor(new Uri(tempFile));

                Assert.AreEqual(fileContentsExpected, fileContentsActual);
            }
        }
    }
}
