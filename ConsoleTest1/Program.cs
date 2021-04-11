using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartPackager;

using System.Reflection.Emit;

namespace ConsoleTest1
{
    class Program
    {
        public struct td
        {
            public int a;
            public string txt;
        }

        static void Main(string[] args)
        {
            Packager packager = new Packager(Pack.Create<td>());

            td BOBA_DATA = new td() { a = 55 , txt="hi"};

            byte[] BOBA_BYTES_ARRAY = new byte[packager.CalcNeedSize(BOBA_DATA)];
            packager.PackUP(BOBA_BYTES_ARRAY, 0, BOBA_DATA);
            //=======[распаковка]=======[BOBA_BYTES_ARRAY можно отправить по сети или сохранить на диск перед распаковкой]==============
            packager.UnPack(BOBA_BYTES_ARRAY, 0, out td biba);

            //вывод
            Console.WriteLine(biba.a);
            Console.WriteLine(biba.txt);

            Console.ReadLine();
        }
    }
}