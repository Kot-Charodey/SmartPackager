using System;
using SmartPackager;

namespace Test
{
    class Program
    {
        public class Ag
        {
            public int gg;
        }

        static void Main()
        {
            var packager = SmartPackager.Packager.Create<Ag>();
            //размер переменной в байтах
            Ag a = new();
            a.gg = 123;
            Console.WriteLine("Size: " + packager.CalcNeedSize(a));
            //упаковать
            byte[] data = packager.PackUP(a);
            //распаковка
            Console.WriteLine("Out:");
            packager.UnPack(data,0,out Ag b);
            Console.WriteLine(b.gg);


            Console.ReadLine();
        }
    }
}
