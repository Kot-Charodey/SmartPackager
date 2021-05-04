using System;
using SmartPackager.Automatic;

namespace Test
{
    class Program
    {
        [SearchPrivateFields]
        public class Ag
        {
            [NotPack]
            private int gg { get; set; } = 5;
            public int gs { get => gg; }
        }

        static void Main()
        {
            var packager = SmartPackager.Packager.Create<Ag>();
            //размер переменной в байтах
            Ag a = new();
            //a.gg;
            Console.WriteLine("Size: " + packager.CalcNeedSize(a));
            //упаковать
            byte[] data = packager.PackUP(a);
            //распаковка
            Console.WriteLine("Out:");
            packager.UnPack(data,0,out Ag b);
            Console.WriteLine(b.gs);


            Console.ReadLine();
        }
    }
}
