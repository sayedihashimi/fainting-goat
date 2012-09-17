namespace fainting.goat.tests {
    using fainting.goat.common;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Ninject;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class TestKernelManager {
        [TestClass]
        public class TheGetKernelMethod{
            [TestMethod]
            public void ReturnsTheKernel() {
                IKernel kernel = KernelManager.GetKernel();
                Assert.IsNotNull(kernel);

                Assert.AreEqual(@"Ninject.StandardKernel", kernel.GetType().FullName);
            }
        }

        [TestClass]
        public class TheSetKernelResolverMethod {
            [TestMethod]
            public void OverridesHowKernelIsCreated() {
                Func<IKernel> creator = () => {
                    return new FakeKernel();
                };

                KernelManager.SetKernelResolver(creator);
                IKernel kernel = KernelManager.GetKernel();

                Assert.AreEqual(@"fainting.goat.tests.FakeKernel", kernel.GetType().FullName);
            }

            [TestMethod]
            public void SettingToNullGoesBackToDefault() {
                Func<IKernel> creator = () => {
                    return new FakeKernel();
                };

                KernelManager.SetKernelResolver(creator);
                IKernel kernel = KernelManager.GetKernel();

                Assert.AreEqual(@"fainting.goat.tests.FakeKernel", kernel.GetType().FullName);

                // set to null to get default behavior
                KernelManager.SetKernelResolver(null);

                kernel = KernelManager.GetKernel();
                Assert.IsNotNull(kernel);

                Assert.AreEqual(@"Ninject.StandardKernel", kernel.GetType().FullName);
            }
        }
    }

    public class FakeKernel : StandardKernel {
    }
}
