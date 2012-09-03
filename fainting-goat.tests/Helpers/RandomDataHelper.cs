namespace fainting.goat.tests.Helpers {
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    public class RandomDataHelper {
        private static RandomDataHelper instance = new RandomDataHelper();

        private RandomDataHelper() {
            this.Primitives = new RandomPrimitives();
        }

        public RandomPrimitives Primitives {
            get;
            private set;
        }

        public static RandomDataHelper Instance { get { return instance; } }

        public IList<T> CreateRandomListOf<T>(Func<T> creator, int maxNumElements) {
            if (creator == null) { throw new System.ArgumentNullException("creator"); }

            int numElements = this.Primitives.GetRandomInt(maxNumElements);

            IList<T> result = new List<T>();
            for (int i = 0; i < maxNumElements; i++) {
                result.Add(creator());
            }
            return result;
        }
    }
}
