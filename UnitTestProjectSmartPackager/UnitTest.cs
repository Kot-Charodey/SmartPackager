using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace UTestSmartPackager
{
    class TestClassData
    {
        public int a;
        public int b;
    }

    [TestClass]
    public class UnitTest
    {
        readonly Random R = new Random();

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

        public class ArrayClassTest
        {
            public int a;
            public ArrayClassTest myRef;
        }

        [TestMethod]
        public void TestPackager_ArrayClass()
        {
            ArrayClassTest[] Arr = new ArrayClassTest[1024];
            ArrayClassTest k = new ArrayClassTest()
            {
                a = R.Next(),
                myRef = null,
            };
            for (int i = 0; i < Arr.Length; i++)
                Arr[i] = new ArrayClassTest() { a = R.Next(), myRef = k };

            var ArrT = TestPackager(Arr);

            Assert.AreEqual(ArrT.Length, Arr.Length);
            for (int i = 0; i < Arr.Length; i++)
            {
                Assert.AreEqual(Arr[i].a, ArrT[i].a);
                Assert.IsTrue(ArrT[0].myRef == ArrT[i].myRef);
                Assert.AreEqual(ArrT[0].myRef.a, ArrT[i].myRef.a);
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
                TestClassData tcd = new TestClassData
                {
                    a = R.Next(),
                    b = R.Next()
                };
                Assert.IsTrue(tcd.a == TestPackager(tcd).a);
                Assert.IsTrue(tcd.b == TestPackager(tcd).b);
            }
        }

        [TestMethod]
        public void TestDictionary()
        {
            SmartPackager.Collections.Generic.Dll.Plug(false);

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

        class RecursivelyClass
        {
            public RecursivelyClass val;
        }


        [TestMethod]
        public void TestRecursivelyClass()
        {
            RecursivelyClass test = new RecursivelyClass();
            test.val = test;

            var pack = SmartPackager.Packager.Create<RecursivelyClass>();
            byte[] data = pack.PackUP(test);
            pack.UnPack(data, 0, out var test2);
            Assert.AreEqual(test2, test2.val);
        }

        class ClassA
        {
            public ClassB a;
            public ClassB b;
        }

        class ClassB
        {
            public int a;
            public ClassA cl;
        }

        [TestMethod]
        public void TestClassRef()
        {
            ClassA test = new ClassA();
            test.a = new ClassB()
            {
                a = 15,
                cl = test
            };
            test.b = test.a;

            var pack = SmartPackager.Packager.Create<ClassA>();
            byte[] data = pack.PackUP(test);
            pack.UnPack(data, 0, out var test2);
            Assert.AreEqual(test2, test2.a.cl);
            Assert.AreEqual(test2, test2.b.cl);
            Assert.AreEqual(test2.a.cl, test2.b.cl);
            Assert.AreEqual(test2.a.a, test2.b.a);
        }

        class ClassC
        {
            public int a;
            public int b;
        }

        [TestMethod]
        public void TestFixedClass()
        {
            ClassC classC = new ClassC
            {
                a = 11,
                b = 22
            };

            var t = TestPackager(classC);
            Assert.AreEqual(t.a, classC.a);
            Assert.AreEqual(t.b, classC.b);
            Assert.AreEqual(SmartPackager.Packager.Create<ClassC>().CalcNeedSize(null), 1);
            Assert.AreEqual(SmartPackager.Packager.Create<ClassC>().CalcNeedSize(new ClassC()), sizeof(byte) + sizeof(int) + sizeof(int));
            Assert.IsTrue(SmartPackager.Packager.IsFixedType<ClassC>());
        }

        [TestMethod]
        public void TestRankArray()
        {
            int[] a = { 1, 2, 3, 4 };
            int[,] b = { { 1, 2, 3 }, { 4, 5, 6 } };
            int[,,] c = { { { 1, 2 }, { 3, 4 } }, { { 5, 6 }, { 7, 8 } } };

            CollectionAssert.AreEqual(TestPackager(a), a);
            CollectionAssert.AreEqual(TestPackager(b), b);
            CollectionAssert.AreEqual(TestPackager(c), c);
        }
    }
}      