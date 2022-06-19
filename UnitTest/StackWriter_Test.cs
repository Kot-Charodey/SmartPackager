using SmartPackager.ByteStack;

namespace UnitTest
{
    //===========================================================//
    //*********** должен быть отестирован UnsafeArray ************
    //===========================================================//
    [TestClass]
    public class StackWriter_Test
    {
        [TestMethod]
        public void Test_WriteNum()
        {
            UnsafeArray.UseArray(new byte[512], 0, 512, (ref UnsafeArray array) =>
            {
                StackWriter writer = new(array);
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
                StackWriter writer = new(array);
                writer.Write(new int[] { 1, 2, 3, 4, 5 });
                CollectionAssert.AreEqual(array.Get<int>(0, 5), new int[] { 1, 2, 3, 4, 5 });
            });
        }

        [TestMethod]
        public void Test_WriteLength()
        {
            UnsafeArray.UseArray(new byte[512], 0, 512, (ref UnsafeArray array) =>
            {
                StackWriter writer = new(array);
                writer.WriteLength(55);
                Assert.AreEqual(array.Get<int>(0), 55);
            });
        }

        [TestMethod]
        public void Test_WriteReference()
        {
            UnsafeArray.UseArray(new byte[512], 0, 512, (ref UnsafeArray array) =>
            {
                StackWriter writer = new(array);
                object obA = new();
                object obB = new();
                writer.MakeReference(obA); //0  - ref data      size 4 type int
                writer.Write(99);          //4  - int 99        size 4 type int
                writer.MakeReference(obA); //8 - ref to 4       size 4 type int
                writer.MakeReference(null);//12 - ref null      size 4 type int
                writer.MakeReference(obB); //16 - ref data      size 4 type int
                writer.Write(15);          //20 - int 15        size 4 type int



                Assert.AreEqual(array.Get<int>(0), RefPoint.DATA);
                Assert.AreEqual(array.Get<int>(4), 99);
                Assert.AreEqual(array.Get<int>(8), 4);
                Assert.AreEqual(array.Get<int>(12), RefPoint.NULL);
                Assert.AreEqual(array.Get<int>(16), RefPoint.DATA);
                Assert.AreEqual(array.Get<int>(20), 15);
            });
        }
    }
}