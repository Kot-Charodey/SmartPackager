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
        static void Main(string[] args)
        {
            Packager packager = new Packager(Pack.Create<int[][]>());

            int[][] BOBA_DATA = { new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { 0, 2, 0 } };
            object[] BOBA_ARRAY = { BOBA_DATA };

            byte[] BOBA_BYTES_ARRAY = new byte[packager.CalcNeedSize(BOBA_ARRAY)];
            packager.PackUP(BOBA_BYTES_ARRAY, 0, BOBA_ARRAY);
            //=======[распаковка]=======[BOBA_BYTES_ARRAY можно отправить по сети или сохранить на диск перед распаковкой]==============
            object[] BOBA_OBJECTS = packager.UnPack(BOBA_BYTES_ARRAY, 0);

            int[][] biba = (int[][])BOBA_OBJECTS[0];
            
            //вывод
            for (int i = 0; i < biba.Length; i++)
                for (int j = 0; j < biba[i].Length; j++)
                    Console.WriteLine(biba[i][j]);

            Console.ReadLine();
        }
    }
}