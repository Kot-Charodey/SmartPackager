using System;

namespace Test
{
    class Program
    {
        class tt
        {
            public int a;
            public int[] b = { 1, 3, 4 };
        }

        static void Main()
        {
            tt t = new tt()
            {
                a = 1234
            };

            var packager = SmartPackager.Packager.Create<tt>();
            Console.WriteLine(packager.CalcNeedSize(t));

            packager.UnPack(packager.PackUP(t),0,out tt res);
            Console.WriteLine(res.a);
            Console.WriteLine(res.b[2]);
            Console.ReadLine();
        }
    }
}
