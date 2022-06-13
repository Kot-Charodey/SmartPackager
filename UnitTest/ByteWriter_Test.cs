using SmartPackager.BitStream;

namespace UnitTest
{
    //===========================================================//
    //*********** должен быть отестирован UnsafeArray ************
    //===========================================================//
    [TestClass]
    public class ByteWriter_Test
    {
        [TestMethod]
        public void Test_WriteNum()
        {
            UnsafeArray.UseArray(new byte[512], 0, 512, (ref UnsafeArray array) =>
            {
                ByteWriter writer = new(array);
                writer.Write(1);
                writer.Write(2);
                writer.Write(3);
                Assert.AreEqual(array.Get<int>(0), 1);
                Assert.AreEqual(array.Get<int>(4), 2);
                Assert.AreEqual(array.Get<int>(8), 3);
            });
        }


        [TestMethod]
        public void Test_WriteArrayNum()
        {
            UnsafeArray.UseArray(new byte[512], 0, 512, (ref UnsafeArray array) =>
            {
                ByteWriter writer = new(array);
                writer.Write(new int[] { 1, 2, 3, 4, 5 });
                CollectionAssert.AreEqual(array.Get<int>(0, 5), new int[] { 1, 2, 3, 4, 5 });
            });
        }

        [TestMethod]
        public void Test_WriteExists()
        {
            UnsafeArray.UseArray(new byte[512], 0, 512, (ref UnsafeArray array) =>
            {
                ByteWriter writer = new(array);
                writer.WriteExists(true);
                Assert.AreEqual(array.Get<bool>(0), true);
            });
        }

        [TestMethod]
        public void Test_WriteLength()
        {
            UnsafeArray.UseArray(new byte[512], 0, 512, (ref UnsafeArray array) =>
            {
                ByteWriter writer = new(array);
                writer.WriteLength(55);
                Assert.AreEqual(array.Get<int>(0), 55);
            });
        }

        [TestMethod]
        public void Test_WriteReference()
        {
            UnsafeArray.UseArray(new byte[512], 0, 512, (ref UnsafeArray array) =>
            {
                ByteWriter writer = new(array);
                writer.WriteReference(new ByteRef(404));
                Assert.AreEqual(array.Get<int>(0), 404);
            });
        }
    }
}
