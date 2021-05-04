#pragma warning disable IDE0059
#pragma warning disable IDE0051
#pragma warning disable CS0649
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
        public const int TestSize = 100000;

        static void Main()
        {
            TestIntArrArrArr16();
            TestIntArr16_16_16();
            TestLongArr16_16_16();
            TestIntClass();
            TestIntStruct();
            TestBigString();
            Console.ReadLine();
        }

        static void TestIntArrArrArr16()
        {
            Console.WriteLine("Test int[16][16][16]");
            Thread.CurrentThread.Priority = ThreadPriority.Highest;

            var packager = Packager.Create<int[][][]>();

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

            Console.WriteLine($"Data size: {packager.CalcNeedSize(data)}\n");

            for (int k = 0; k < TestCount; k++)
            {
                Console.WriteLine($"Measurement № {k + 1} / {TestCount}");
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                for (int i = 0; i < TestSize; i++)
                {
                    byte[] bytes = packager.PackUP(data);
                    packager.UnPack(bytes, 0, out int[][][] t1);
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

            var packager = Packager.Create<int[]>();

            int[] data = new int[16 * 16 * 16];

            for (int i = 0; i < 16 * 16 * 16; i++)
            {
                data[i] = i;
            }

            Console.WriteLine($"Data size: {packager.CalcNeedSize(data)}\n");

            for (int k = 0; k < TestCount; k++)
            {
                Console.WriteLine($"Measurement № {k + 1} / {TestCount}");
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                for (int i = 0; i < TestSize; i++)
                {
                    byte[] bytes = packager.PackUP(data);
                    packager.UnPack(bytes, 0, out int[] dd);
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

            var packager = Packager.Create<long[]>();

            long[] data = new long[16 * 16 * 16];

            for (long i = 0; i < 16 * 16 * 16; i++)
            {
                data[i] = i;
            }

            Console.WriteLine($"Data size: {packager.CalcNeedSize(data)}\n");

            for (int k = 0; k < TestCount; k++)
            {
                Console.WriteLine($"Measurement № {k + 1} / {TestCount}");
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                for (int i = 0; i < TestSize; i++)
                {
                    byte[] bytes = packager.PackUP(data);
                    packager.UnPack(bytes, 0, out long[] dd);
                }
                stopwatch.Stop();
                Console.WriteLine($"Time spent per unit: {stopwatch.Elapsed.TotalMilliseconds / TestSize} m/s");
            }
            Console.WriteLine("Done\n\n");
        }

        static void TestIntClass()
        {
            Console.WriteLine($"Test IntClass");
            Thread.CurrentThread.Priority = ThreadPriority.Highest;

            var packager = Packager.Create<IntClass>();

            IntClass data = new IntClass();

            Console.WriteLine($"Data size: {packager.CalcNeedSize(data)}\n");

            for (int k = 0; k < TestCount; k++)
            {
                Console.WriteLine($"Measurement № {k + 1} / {TestCount}");
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                for (int i = 0; i < TestSize; i++)
                {
                    byte[] bytes = packager.PackUP(data);
                    packager.UnPack(bytes, 0, out IntClass dd);
                }
                stopwatch.Stop();
                Console.WriteLine($"Time spent per unit: {stopwatch.Elapsed.TotalMilliseconds / TestSize} m/s");
            }
            Console.WriteLine("Done\n\n");
        }

        static void TestIntStruct()
        {
            Console.WriteLine($"Test IntStruct");
            Thread.CurrentThread.Priority = ThreadPriority.Highest;

            var packager = Packager.Create<IntStruct>();

            IntStruct data = new IntStruct();
            data.Init(3384);

            Console.WriteLine($"Data size: {packager.CalcNeedSize(data)}\n");

            for (int k = 0; k < TestCount; k++)
            {
                Console.WriteLine($"Measurement № {k + 1} / {TestCount}");
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                for (int i = 0; i < TestSize; i++)
                {
                    byte[] bytes = packager.PackUP(data);
                    packager.UnPack(bytes, 0, out IntStruct dd);
                }
                stopwatch.Stop();
                Console.WriteLine($"Time spent per unit: {stopwatch.Elapsed.TotalMilliseconds / TestSize} m/s");
            }
            Console.WriteLine("Done\n\n");
        }

        static void TestBigString()
        {
            Console.WriteLine($"Test BigString");
            Thread.CurrentThread.Priority = ThreadPriority.Highest;

            var packager = Packager.Create<string>();

            string data = BigString.GetString();

            Console.WriteLine($"Data size: {packager.CalcNeedSize(data)}\n");

            for (int k = 0; k < TestCount; k++)
            {
                Console.WriteLine($"Measurement № {k + 1} / {TestCount}");
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                for (int i = 0; i < TestSize; i++)
                {
                    byte[] bytes = packager.PackUP(data);
                    packager.UnPack(bytes, 0, out string dd);
                }
                stopwatch.Stop();
                Console.WriteLine($"Time spent per unit: {stopwatch.Elapsed.TotalMilliseconds / TestSize} m/s");
            }
            Console.WriteLine("Done\n\n");
        }
    }
}