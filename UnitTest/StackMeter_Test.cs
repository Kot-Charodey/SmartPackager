using SmartPackager.ByteStack;

namespace UnitTest
{
    [TestClass]
    public class StackMeter_Test
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
        public void Test_AddReference()
        {
            StackMeter Meter = new();
            object obA = new();
            object obB = new();
            Meter.MakeReference(obA);//4
            Meter.AddFixedSize(6);//4+6
            Meter.MakeReference(obA);//4+6+4
            Meter.MakeReference(null);//4+6+4+4
            Meter.MakeReference(obB);//4+6+4+4+4
            Meter.AddFixedSize(2);//4+6+4+4+4+2

            Assert.AreEqual(Meter.GetCalcLength(), 4 + 6 + 4 + 4 + 4 + 2);
        }
    }
}
