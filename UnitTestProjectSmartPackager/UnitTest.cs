using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;

namespace UnitTestProjectSmartPackager
{
    class TestClassData
    {
        public int a;
        public int b;
    }

    [TestClass]
    public class UnitTest
    {
        Random R = new Random();

        private T TestPackager<T>(T data)
        {
            var packager = SmartPackager.Packager.Create<T>();
            byte[] bytes = packager.PackUP(data);
            packager.UnPack(bytes, 0, out var Pdata);
            return Pdata;
        }

        [TestMethod]
        public void TestPackager_Numeric()
        {
            Assert.AreEqual(byte.MinValue,
               TestPackager(byte.MinValue));
            Assert.AreEqual(byte.MaxValue,
               TestPackager(byte.MaxValue));
            Assert.AreEqual(byte.MinValue / 2,
               TestPackager(byte.MinValue / 2));

            Assert.AreEqual(sbyte.MinValue,
               TestPackager(sbyte.MinValue));
            Assert.AreEqual(sbyte.MaxValue,
               TestPackager(sbyte.MaxValue));
            Assert.AreEqual(sbyte.MinValue / 2,
               TestPackager(sbyte.MinValue / 2));

            Assert.AreEqual(ushort.MinValue,
               TestPackager(ushort.MinValue));
            Assert.AreEqual(ushort.MaxValue,
               TestPackager(ushort.MaxValue));
            Assert.AreEqual(ushort.MinValue / 2,
               TestPackager(ushort.MinValue / 2));

            Assert.AreEqual(short.MinValue,
               TestPackager(short.MinValue));
            Assert.AreEqual(short.MaxValue,
               TestPackager(short.MaxValue));
            Assert.AreEqual(short.MinValue / 2,
               TestPackager(short.MinValue / 2));

            Assert.AreEqual(uint.MinValue,
               TestPackager(uint.MinValue));
            Assert.AreEqual(uint.MaxValue,
               TestPackager(uint.MaxValue));
            Assert.AreEqual(uint.MinValue / 2,
               TestPackager(uint.MinValue / 2));

            Assert.AreEqual(int.MinValue,
               TestPackager(int.MinValue));
            Assert.AreEqual(int.MaxValue,
               TestPackager(int.MaxValue));
            Assert.AreEqual(int.MinValue / 2,
               TestPackager(int.MinValue / 2));

            Assert.AreEqual(ulong.MinValue,
               TestPackager(ulong.MinValue));
            Assert.AreEqual(ulong.MaxValue,
               TestPackager(ulong.MaxValue));
            Assert.AreEqual(ulong.MinValue / 2,
               TestPackager(ulong.MinValue / 2));

            Assert.AreEqual(long.MinValue,
               TestPackager(long.MinValue));
            Assert.AreEqual(long.MaxValue,
               TestPackager(long.MaxValue));
            Assert.AreEqual(long.MinValue / 2,
               TestPackager(long.MinValue / 2));

            Assert.AreEqual(float.MinValue,
               TestPackager(float.MinValue));
            Assert.AreEqual(float.MaxValue,
               TestPackager(float.MaxValue));
            Assert.AreEqual(float.MinValue / 2,
               TestPackager(float.MinValue / 2));

            Assert.AreEqual(double.MinValue,
               TestPackager(double.MinValue));
            Assert.AreEqual(double.MaxValue,
               TestPackager(double.MaxValue));
            Assert.AreEqual(double.MinValue / 2,
               TestPackager(double.MinValue / 2));

            Assert.AreEqual(decimal.MinValue,
               TestPackager(decimal.MinValue));
            Assert.AreEqual(decimal.MaxValue,
               TestPackager(decimal.MaxValue));
            Assert.AreEqual(decimal.MinValue / 2,
               TestPackager(decimal.MinValue / 2));
        }

        [TestMethod]
        public void TestPackager_CustomType_UInt24()
        {
            Assert.AreEqual(UInt24.MinValue,
               TestPackager(UInt24.MinValue));
            Assert.AreEqual(UInt24.MaxValue,
               TestPackager(UInt24.MaxValue));
            Assert.AreEqual(UInt24.MinValue / 2,
               TestPackager(UInt24.MinValue / 2));
        }

        [TestMethod]
        public void TestPackager_ArrayFixed()
        {
            {
                int[] Arr = null;
                Assert.AreEqual(Arr,
                   TestPackager(Arr));
            }
            {
                int[] Arr = new int[0];
                CollectionAssert.AreEqual(Arr,
                   TestPackager(Arr));
            }
            {
                int[] Arr = new int[1];
                CollectionAssert.AreEqual(Arr,
                   TestPackager(Arr));
            }
            {
                byte[] Arr = new byte[ushort.MaxValue];
                CollectionAssert.AreEqual(Arr,
                   TestPackager(Arr));
            }
            {
                int[] Arr = new int[] { R.Next() };
                CollectionAssert.AreEqual(Arr,
                   TestPackager(Arr));
            }
            {
                int[] Arr = new int[1024];
                for (int i = 0; i < Arr.Length; i++)
                    Arr[i] = R.Next();
                CollectionAssert.AreEqual(Arr,
                   TestPackager(Arr));
            }
        }

        [TestMethod]
        public void TestPackager_ArrayArray()
        {
            {
                int[][] Arr = null;
                Assert.AreEqual(Arr,
                   TestPackager(Arr));
            }
            {
                int[][] Arr = new int[0][];
                CollectionAssert.AreEqual(Arr,
                   TestPackager(Arr));
            }
            {
                int[][] Arr = new int[1][];
                Arr[0] = new int[0];
                Assert.IsTrue(Arr[0].Length == TestPackager(Arr)[0].Length);
            }
            {
                int[][] Arr = new int[1][];
                Arr[0] = new int[] { R.Next() };
                Assert.IsTrue(Arr[0][0] == TestPackager(Arr)[0][0]);
            }
        }

        [TestMethod]
        public void TestPackager_Class()
        {
            {
                TestClassData tcd = null;
                Assert.IsTrue(TestPackager(tcd) == null);
            }
            {
                TestClassData tcd = new TestClassData();

                Assert.IsTrue(tcd.a == TestPackager(tcd).a);
                Assert.IsTrue(tcd.b == TestPackager(tcd).b);
            }
            {
                TestClassData tcd = new TestClassData();
                tcd.a = R.Next();
                tcd.b = R.Next();
                Assert.IsTrue(tcd.a == TestPackager(tcd).a);
                Assert.IsTrue(tcd.b == TestPackager(tcd).b);
            }
        }

        [TestMethod]
        public void TestDictionary()
        {
            SmartPackager.Collections.Generic.Dll.Plug();

            var pack = SmartPackager.Packager.Create<Dictionary<string, int>>();
            Assert.AreEqual(pack.CalcNeedSize(null), 1);
            Assert.IsTrue(pack.CalcNeedSize(new Dictionary<string, int>()) > 1);

            pack.UnPack(pack.PackUP(null), 0, out var testD0);
            Assert.IsNull(testD0);

            var testD = new Dictionary<string, int>
            {
                { "a", 1 },
                { "b", 2 },
                { "c", 3 }
            };

            pack.UnPack(pack.PackUP(testD), 0, out testD);
            Assert.AreEqual(testD["a"], 1);
            Assert.AreEqual(testD["b"], 2);
            Assert.AreEqual(testD["c"], 3);
        }
    }
}