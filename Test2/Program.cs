using System;
using SmartPackager.Automatic;

namespace Test
{
    class Program
    {
        struct TestClassData
        {
            public string g;
        }

        static void Main()
        {
            TestClassData tcd = new TestClassData() { g = "boba" };

            var packager = SmartPackager.Packager.Create<TestClassData>();
            Console.WriteLine("Size: " + packager.CalcNeedSize(tcd));
            //упаковать
            byte[] data = packager.PackUP(tcd);
            //распаковка
            Console.WriteLine("Out:");
            packager.UnPack(data, 0, out var b);
            Console.WriteLine(b.g);


            Console.ReadLine();
        }
    }
}