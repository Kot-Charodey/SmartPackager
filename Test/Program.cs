using System;

namespace Test
{
    class Program
    {
        class tt //size 1
        {
            public int a; //size 4
            public int[] b = { 1, 3, 4 }; //size 12 + 4 (Length)
        }// 1 + 4 + 16 = 21 byte

        static void Main()
        {
            tt t = new()
            {
                a = 1234
            };

            var packager = SmartPackager.Packager.Create<tt>();
            Console.WriteLine("Size: "+packager.CalcNeedSize(t));

            packager.UnPack(packager.PackUP(t),0,out tt res);
            Console.WriteLine(res.a);
                foreach(var bb in res.b)
                 Console.Write(bb);
            Console.ReadLine();
        }
    }
}
