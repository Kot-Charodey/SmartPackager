using SmartPackager.ByteStack;

namespace UnitTest
{
    [TestClass]
    public class ByteMeter_Test
    {
        [TestMethod]
        public void Test_CalcLength()
        {
            StackMeter Meter = new();
            Assert.AreEqual(Meter.GetCalcLength(), 0);
        }
        [TestMethod]
        public void Test_Add()
        {
            StackMeter Meter = new();
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
            StackMeter Meter = new();
            Meter.Add<int>(5);
            Assert.AreEqual(Meter.GetCalcLength(), 4 * 5);
            Meter.Add<long>();
            Assert.AreEqual(Meter.GetCalcLength(), 4 * 5 + 8);
        }
        [TestMethod]
        public void Test_AddLength()
        {
            StackMeter Meter = new();
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
            StackMeter Meter = new();
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
            StackMeter Meter = new();
            Meter.Add<int>();
            Assert.AreEqual(Meter.GetCalcLength(), 4);
            var @ref = Meter.MakeReference();
            Assert.AreEqual(Meter.GetCalcLength(), 8);
            Assert.AreEqual(@ref.GetPoint(), 4);
            Meter.Add<long>();
            Assert.AreEqual(Meter.GetCalcLength(), 16);
        }
    }
}
