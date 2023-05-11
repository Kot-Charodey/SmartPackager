using SmartPackager.ByteStack;

namespace UnitTest
{
    //===========================================================//
    //*********** должен быть отестирован UnsafeArray ************
    //*********** должен быть отестирован StackWriter  ************
    //===========================================================//
    [TestClass]
    public class StackReader_Test
    {
        [TestMethod]
        public void Test_ReadNum()
        {
            var arr = new byte[512];
            UnsafeArray.UseArray(arr, 0, (ref UnsafeArray array) =>
            {
                StackWriter writer = new(array);
                writer.Write(1);
                writer.Write(2);
                writer.Write(3);
            });

            UnsafeArray.UseArray(arr, 0, (ref UnsafeArray array) =>
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
            UnsafeArray.UseArray(arr, 0, (ref UnsafeArray array) =>
            {
                StackWriter writer = new(array);
                writer.Write(testArr);
            });

            UnsafeArray.UseArray(arr, 0, (ref UnsafeArray array) =>
            {
                StackReader reader = new(array);
                CollectionAssert.AreEqual(reader.Read<int>(testArr.Length), testArr);
            });
        }

        [TestMethod]
        public void Test_ReadLength()
        {
            var arr = new byte[512];
            UnsafeArray.UseArray(arr, 0, (ref UnsafeArray array) =>
            {
                StackWriter writer = new(array);
                writer.WriteLength(5);
                writer.WriteLength(25);
                writer.WriteLength(-1);
            });

            UnsafeArray.UseArray(arr, 0, (ref UnsafeArray array) =>
            {
                StackReader reader = new(array);
                Assert.AreEqual(reader.ReadLength(), 5);
                Assert.AreEqual(reader.ReadLength(), 25);
                Assert.AreEqual(reader.ReadLength(), -1);
            });
        }
    }
}