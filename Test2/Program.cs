using System;
using System.Collections.Generic;
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
            SmartPackager.Collections.Generic.Dll.Plug();
            var t=SmartPackager.PackMethods.GetPackMethods();

            var packager = SmartPackager.Packager.Create<Dictionary<string,int>>();
            var d=new Dictionary<string,int>();
            d.Add("a", 1);
            d.Add("b", 2);
            d.Add("c", 3);
            Console.WriteLine("Size: " + packager.CalcNeedSize(d));
            //упаковать
            byte[] data = packager.PackUP(d);
            //распаковка
            Console.WriteLine("Out:");
            packager.UnPack(data, 0, out var b);
            Console.WriteLine(b["a"]);
            Console.WriteLine(b["b"]);
            Console.WriteLine(b["c"]);


            Console.ReadLine();
        }
    }
}