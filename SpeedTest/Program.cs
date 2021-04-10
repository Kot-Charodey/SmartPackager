using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using SmartPackager;

namespace SpeedTest
{
    class Program
    {
        public const int TestCount = 3;
        public const int TestSize = 10000;

        static void Main(string[] args)
        {
            TestIntArrArrArr16();
            TestIntArr16_16_16();
            TestLongArr16_16_16();
            Console.ReadLine();
        }

        static void TestIntArrArrArr16()
        {
            Console.WriteLine("Test int[16][16][16]");
            Thread.CurrentThread.Priority = ThreadPriority.Highest;

            Packager packager = new Packager(Pack.Create<int[][][]>());

            int[][][] data = new int[16][][];

            for (int i = 0; i < 16; i++)
            {
                data[i] = new int[16][];
                for (int j = 0; j < 16; j++)
                {
                    data[i][j] = new int[16];
                    for (int k = 0; k < 16; k++)
                    {
                        data[i][j][k] = i * j * k;
                    }
                }
            }

            object[] data_arr = { data };

            Console.WriteLine($"Data size: {packager.CalcNeedSize(data_arr)}\n");

            for (int k = 0; k < TestCount; k++)
            {
                Console.WriteLine($"Measurement № {k + 1} / {TestCount}");
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                for (int i = 0; i < TestSize; i++)
                {
                    byte[] bytes = new byte[packager.CalcNeedSize(data_arr)];
                    packager.PackUP(bytes, 0, data_arr);
                    object[] dataNew = packager.UnPack(bytes, 0);
                }
                stopwatch.Stop();
                Console.WriteLine($"Time spent per unit: {stopwatch.Elapsed.TotalMilliseconds / TestSize} m/s");
            }
            Console.WriteLine("Done\n\n");
        }

        static void TestIntArr16_16_16()
        {
            Console.WriteLine($"Test int[{16 * 16 * 16}]");
            Thread.CurrentThread.Priority = ThreadPriority.Highest;

            Packager packager = new Packager(Pack.Create<int[]>());

            int[] data = new int[16*16*16];

            for (int i = 0; i < 16*16*16; i++)
            {
                data[i] = i;
            }

            object[] data_arr = { data };

            Console.WriteLine($"Data size: {packager.CalcNeedSize(data_arr)}\n");

            for (int k = 0; k < TestCount; k++)
            {
                Console.WriteLine($"Measurement № {k + 1} / {TestCount}");
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                for (int i = 0; i < TestSize; i++)
                {
                    byte[] bytes = new byte[packager.CalcNeedSize(data_arr)];
                    packager.PackUP(bytes, 0, data_arr);
                    object[] dataNew = packager.UnPack(bytes, 0);
                }
                stopwatch.Stop();
                Console.WriteLine($"Time spent per unit: {stopwatch.Elapsed.TotalMilliseconds / TestSize} m/s");
            }
            Console.WriteLine("Done\n\n");
        }

        static void TestLongArr16_16_16()
        {
            Console.WriteLine($"Test long[{16 * 16 * 16}]");
            Thread.CurrentThread.Priority = ThreadPriority.Highest;

            Packager packager = new Packager(Pack.Create<long[]>());

            long[] data = new long[16 * 16 * 16];

            for (long i = 0; i < 16 * 16 * 16; i++)
            {
                data[i] = i;
            }

            object[] data_arr = { data };

            Console.WriteLine($"Data size: {packager.CalcNeedSize(data_arr)}\n");

            for (int k = 0; k < TestCount; k++)
            {
                Console.WriteLine($"Measurement № {k + 1} / {TestCount}");
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                for (int i = 0; i < TestSize; i++)
                {
                    byte[] bytes = new byte[packager.CalcNeedSize(data_arr)];
                    packager.PackUP(bytes, 0, data_arr);
                    object[] dataNew = packager.UnPack(bytes, 0);
                }
                stopwatch.Stop();
                Console.WriteLine($"Time spent per unit: {stopwatch.Elapsed.TotalMilliseconds / TestSize} m/s");
            }
            Console.WriteLine("Done\n\n");
        }
    }
}