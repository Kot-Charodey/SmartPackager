using SmartPackager.BitStream;

namespace UnitTest
{
    [TestClass]
    public class ByteMeter_Test
    {
        [TestMethod]
        public void Test_CalcLength()
        {
            ByteMeter Meter = new();
            Assert.AreEqual(Meter.GetCalcLength(), 0);
        }
        [TestMethod]
        public void Test_Add()
        {
            ByteMeter Meter = new();
            Meter.Add<int>();
            Assert.AreEqual(Meter.GetCalcLength(), 4);
            Meter.Add<int>();
            Assert.AreEqual(Meter.GetCalcLength(), 8);
            Meter.Add<long>();
            Assert.AreEqual(Meter.GetCalcLength(), 16);
        }
        [TestMethod]
        public void Test_AddArray()
        {
            ByteMeter Meter = new();
            Meter.Add<int>(5);
            Assert.AreEqual(Meter.GetCalcLength(), 4 * 5);
            Meter.Add<long>();
            Assert.AreEqual(Meter.GetCalcLength(), 4 * 5 + 8);
        }
        [TestMethod]
        public void Test_AddLength()
        {
            ByteMeter Meter = new();
            Meter.Add<int>();
            Assert.AreEqual(Meter.GetCalcLength(), 4);
            Meter.AddLength();
            Assert.AreEqual(Meter.GetCalcLength(), 8);
            Meter.Add<long>();
            Assert.AreEqual(Meter.GetCalcLength(), 16);
        }
        [TestMethod]
        public void Test_AddExists()
        {
            ByteMeter Meter = new();
            Meter.Add<int>();
            Assert.AreEqual(Meter.GetCalcLength(), 4);
            Meter.AddExists();
            Assert.AreEqual(Meter.GetCalcLength(), 5);
            Meter.Add<long>();
            Assert.AreEqual(Meter.GetCalcLength(), 13);
        }
        [TestMethod]
        public void Test_AddReference()
        {
            ByteMeter Meter = new();
            Meter.Add<int>();
            Assert.AreEqual(Meter.GetCalcLength(), 4);
            Meter.AddReference();
            Assert.AreEqual(Meter.GetCalcLength(), 8);
            Meter.Add<long>();
            Assert.AreEqual(Meter.GetCalcLength(), 16);
        }
    }
}
