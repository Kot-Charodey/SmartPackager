using SmartPackager.ByteStack;

namespace UnitTest
{
    [TestClass]
    public class ByteRef_Test
    {
        [TestMethod]
        public void AreEqual()
        {
            ByteRef a = new (15);
            ByteRef b = new (15);
            ByteRef c = new (25);

            Assert.AreEqual(a, a);
            Assert.AreEqual(a, b);
            Assert.AreNotEqual(a, c);
        }

        [TestMethod]
        public void GetPoint()
        {
            ByteRef a = new (15);
            ByteRef b = new (int.MaxValue);
            ByteRef c = new (25);

            Assert.AreEqual(a.GetPoint(), 15);
            Assert.AreEqual(b.GetPoint(), int.MaxValue);
            Assert.AreEqual(c.GetPoint(), 25);
        }
    }
}