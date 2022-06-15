using SmartPackager.ByteStack;

namespace UnitTest
{
    //===========================================================//
    //*********** должен быть отестирован UnsafeArray ************
    //*********** должен быть отестирован StackWriter  ************
    //===========================================================//
    [TestClass]
    public class ByteReader_Test
    {
        [TestMethod]
        public void Test_ReadNum()
        {
            var arr = new byte[512];
            UnsafeArray.UseArray(arr, 0, arr.Length, (ref UnsafeArray array) =>
            {
                StackWriter writer = new(array);
                writer.Write(1);
                writer.Write(2);
                writer.Write(3);
            });

            UnsafeArray.UseArray(arr, 0, arr.Length, (ref UnsafeArray array) =>
            {
                StackReader reader = new(array);
                Assert.AreEqual(reader.Read<int>(), 1);
                Assert.AreEqual(reader.Read<int>(), 2);
                Assert.AreEqual(reader.Read<int>(), 3);
            });
        }

        [TestMethod]
        public void Test_ReadArrayNum()
        {
            var arr = new byte[512];
            var testArr = new int[] { 1, 2, 3, 4, 5 };
            UnsafeArray.UseArray(arr, 0, arr.Length, (ref UnsafeArray array) =>
            {
                StackWriter writer = new(array);
                writer.Write(testArr);
            });

            UnsafeArray.UseArray(arr, 0, arr.Length, (ref UnsafeArray array) =>
            {
                StackReader reader = new(array);
                CollectionAssert.AreEqual(reader.Read<int>(testArr.Length), testArr);
            });
        }

        [TestMethod]
        public void Test_ReadExist()
        {
            var arr = new byte[512];
            UnsafeArray.UseArray(arr, 0, arr.Length, (ref UnsafeArray array) =>
            {
                StackWriter writer = new(array);
                writer.WriteExists(true);
                writer.WriteExists(false);
            });

            UnsafeArray.UseArray(arr, 0, arr.Length, (ref UnsafeArray array) =>
            {
                StackReader reader = new(array);
                Assert.AreEqual(reader.ReadExists(), true);
                Assert.AreEqual(reader.ReadExists(), false);
            });
        }

        [TestMethod]
        public void Test_ReadLength()
        {
            var arr = new byte[512];
            UnsafeArray.UseArray(arr, 0, arr.Length, (ref UnsafeArray array) =>
            {
                StackWriter writer = new(array);
                writer.WriteLength(5);
                writer.WriteLength(25);
                writer.WriteLength(-1);
            });

            UnsafeArray.UseArray(arr, 0, arr.Length, (ref UnsafeArray array) =>
            {
                StackReader reader = new(array);
                Assert.AreEqual(reader.ReadLength(), 5);
                Assert.AreEqual(reader.ReadLength(), 25);
                Assert.AreEqual(reader.ReadLength(), -1);
            });
        }


        [TestMethod]
        public void Test_ReadReference()
        {
            var arr = new byte[512];
            UnsafeArray.UseArray(arr, 0, arr.Length, (ref UnsafeArray array) =>
            {
                StackWriter writer = new(array);
                writer.WriteLength(5);
                var @ref = writer.MakeReference();
                writer.WriteLength(25);
                writer.WriteLength(-1);
                writer.WriteReference(@ref);
                writer.WriteLength(123);
            });

            UnsafeArray.UseArray(arr, 0, arr.Length, (ref UnsafeArray array) =>
            {
                StackReader reader = new(array);
                Assert.AreEqual(reader.ReadLength(), 5);
                Assert.AreEqual(reader.ReadLength(), 25);
                Assert.AreEqual(reader.ReadLength(), -1);

                var @ref = reader.ReadReference();
                Assert.AreEqual(@ref.GetPoint(), 4);
                Assert.AreEqual(reader.ReadLength(), 123);
            });
        }
    }
}