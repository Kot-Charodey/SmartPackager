using SmartPackager.BitStream;

namespace UnitTest
{
    [TestClass]
    public unsafe class UnsafeArray_Test
    {
        private static void InvokeIsError(ref UnsafeArray array, UnsafeArrayAction action)
        {
            try
            {
                action(ref array);
            }
            catch
            {
                Assert.IsTrue(true);
                return;
            }
            Assert.IsTrue(false);
        }

        [TestMethod]
        public void Test_SetGetNum()
        {
            byte[] bytes = new byte[1024];
            UnsafeArray.UseArray(bytes, 0, 8, (ref UnsafeArray uArray) =>
            {
                int a = 15;
                int b = int.MaxValue;
                uArray.Set(0, a);
                uArray.Set(4, b);
                Assert.AreEqual(a, uArray.Get<int>(0));
                Assert.AreEqual(b, uArray.Get<int>(4));
            });
        }

        [TestMethod]
        public void Test_SetGetNum_Error()
        {
            byte[] bytes = new byte[1024];
            UnsafeArray.UseArray(bytes, 0, 0, (ref UnsafeArray uArray) =>
            {
                int a = 15;
                InvokeIsError(ref uArray, (ref UnsafeArray arr) => arr.Set(4, a));
                InvokeIsError(ref uArray, (ref UnsafeArray arr) => arr.Get<int>(4));
            });
        }


        [TestMethod]
        public void Test_SetGetArrayNum()
        {
            byte[] bytes = new byte[1024];
            UnsafeArray.UseArray(bytes, 0, 4*5, (ref UnsafeArray uArray) =>
            {
                int[] a = { int.MaxValue, int.MinValue, 3, 25, -0 };
                uArray.Set(0, a);
                CollectionAssert.AreEqual(a, uArray.Get<int>(0, a.Length));
            });
        }


        [TestMethod]
        public void Test_SetGetArrayNum_Error()
        {
            byte[] bytes = new byte[1024];
            UnsafeArray.UseArray(bytes, 0, 4 * 5, (ref UnsafeArray uArray) =>
            {
                int[] a = { int.MaxValue, int.MinValue, 3, 25, -0 };
                InvokeIsError(ref uArray, (ref UnsafeArray arr) => arr.Set(1, a));
            });
        }
    }
}