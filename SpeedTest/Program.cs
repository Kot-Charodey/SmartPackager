﻿#pragma warning disable IDE0059
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
            //TestIntArrArrArr16();
            //TestIntArr16_16_16();
            //TestLongArr16_16_16();
            TestIntClass();
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
        }

        public class IntClass
        {
            public long a1f = 5;         public long a1f1 = 5;         public long a1f2 = 5;           public long a1f3 = 5;
            public long a2f = 5;         public long a2f1 = 5;         public long a2f2 = 5;           public long a2f3 = 5;
            public long a3f = 5;         public long a3f1 = 5;         public long a3f2 = 5;           public long a3f3 = 5;
            public long a4f = 5;         public long a4f1 = 5;         public long a4f2 = 5;           public long a4f3 = 5;
            public long a5f = 5;         public long a5f1 = 5;         public long a5f2 = 5;           public long a5f3 = 5;
            public long a6f = 5;         public long a6f1 = 5;         public long a6f2 = 5;           public long a6f3 = 5;
            public long a7f = 5;         public long a7f1 = 5;         public long a7f2 = 5;           public long a7f3 = 5;
            public long a8f = 5;         public long a8f1 = 5;         public long a8f2 = 5;           public long a8f3 = 5;
            public long a9f = 5;         public long a9f1 = 5;         public long a9f2 = 5;           public long a9f3 = 5;
            public long a11 = 5;         public long a111 = 5;         public long a112 = 5;           public long a113 = 5;
            public long a21 = 5;         public long a211 = 5;         public long a212 = 5;           public long a213 = 5;
            public long a31 = 5;         public long a311 = 5;         public long a312 = 5;           public long a313 = 5;
            public long a41 = 5;         public long a411 = 5;         public long a412 = 5;           public long a413 = 5;
            public long a51 = 5;         public long a511 = 5;         public long a512 = 5;           public long a513 = 5;
            public long a61 = 5;         public long a611 = 5;         public long a612 = 5;           public long a613 = 5;
            public long a71 = 5;         public long a711 = 5;         public long a712 = 5;           public long a713 = 5;
            public long a81 = 5;         public long a811 = 5;         public long a812 = 5;           public long a813 = 5;
            public long a91 = 5;         public long a911 = 5;         public long a912 = 5;           public long a913 = 5;
            public long a12 = 5;         public long a121 = 5;         public long a122 = 5;           public long a123 = 5;
            public long a22 = 5;         public long a221 = 5;         public long a222 = 5;           public long a223 = 5;
            public long a32 = 5;         public long a321 = 5;         public long a322 = 5;           public long a323 = 5;
            public long a42 = 5;         public long a421 = 5;         public long a422 = 5;           public long a423 = 5;
            public long a52 = 5;         public long a521 = 5;         public long a522 = 5;           public long a523 = 5;
            public long a62 = 5;         public long a621 = 5;         public long a622 = 5;           public long a623 = 5;
            public long a72 = 5;         public long a721 = 5;         public long a722 = 5;           public long a723 = 5;
            public long a82 = 5;         public long a821 = 5;         public long a822 = 5;           public long a823 = 5;
            public long a92 = 5;         public long a921 = 5;         public long a922 = 5;           public long a923 = 5;
            public long a13 = 5;         public long a131 = 5;         public long a132 = 5;           public long a133 = 5;
            public long a23 = 5;         public long a231 = 5;         public long a232 = 5;           public long a233 = 5;
            public long a33 = 5;         public long a331 = 5;         public long a332 = 5;           public long a333 = 5;
            public long a43 = 5;         public long a431 = 5;         public long a432 = 5;           public long a433 = 5;
            public long a53 = 5;         public long a531 = 5;         public long a532 = 5;           public long a533 = 5;
            public long a63 = 5;         public long a631 = 5;         public long a632 = 5;           public long a633 = 5;
            public long a73 = 5;         public long a731 = 5;         public long a732 = 5;           public long a733 = 5;
            public long a83 = 5;         public long a831 = 5;         public long a832 = 5;           public long a833 = 5;
            public long a93 = 5;         public long a931 = 5;         public long a932 = 5;           public long a933 = 5;
            public long a14 = 5;         public long a141 = 5;         public long a142 = 5;           public long a143 = 5;
            public long a24 = 5;         public long a241 = 5;         public long a242 = 5;           public long a243 = 5;
            public long a34 = 5;         public long a341 = 5;         public long a342 = 5;           public long a343 = 5;
            public long a44 = 5;         public long a441 = 5;         public long a442 = 5;           public long a443 = 5;
            public long a54 = 5;         public long a541 = 5;         public long a542 = 5;           public long a543 = 5;
            public long a64 = 5;         public long a641 = 5;         public long a642 = 5;           public long a643 = 5;
            public long a74 = 5;         public long a741 = 5;         public long a742 = 5;           public long a743 = 5;
            public long a84 = 5;         public long a841 = 5;         public long a842 = 5;           public long a843 = 5;
            public long a94 = 5;         public long a941 = 5;         public long a942 = 5;           public long a943 = 5;
            public long a15 = 5;         public long a151 = 5;         public long a152 = 5;           public long a153 = 5;
            public long a25 = 5;         public long a251 = 5;         public long a252 = 5;           public long a253 = 5;
            public long a35 = 5;         public long a351 = 5;         public long a352 = 5;           public long a353 = 5;
            public long a45 = 5;         public long a451 = 5;         public long a452 = 5;           public long a453 = 5;
            public long a55 = 5;         public long a551 = 5;         public long a552 = 5;           public long a553 = 5;
            public long a65 = 5;         public long a651 = 5;         public long a652 = 5;           public long a653 = 5;
            public long a75 = 5;         public long a751 = 5;         public long a752 = 5;           public long a753 = 5;
            public long a85 = 5;         public long a851 = 5;         public long a852 = 5;           public long a853 = 5;
            public long a95 = 5;         public long a951 = 5;         public long a952 = 5;           public long a953 = 5;
            public long a16 = 5;         public long a161 = 5;         public long a162 = 5;           public long a163 = 5;
            public long a26 = 5;         public long a261 = 5;         public long a262 = 5;           public long a263 = 5;
            public long a36 = 5;         public long a361 = 5;         public long a362 = 5;           public long a363 = 5;
            public long a46 = 5;         public long a461 = 5;         public long a462 = 5;           public long a463 = 5;
            public long a56 = 5;         public long a561 = 5;         public long a562 = 5;           public long a563 = 5;
            public long a66 = 5;         public long a661 = 5;         public long a662 = 5;           public long a663 = 5;
            public long a76 = 5;         public long a761 = 5;         public long a762 = 5;           public long a763 = 5;
            public long a86 = 5;         public long a861 = 5;         public long a862 = 5;           public long a863 = 5;
            public long a96 = 5;         public long a961 = 5;         public long a962 = 5;           public long a963 = 5;
            public long a17 = 5;         public long a171 = 5;         public long a172 = 5;           public long a173 = 5;
            public long a27 = 5;         public long a271 = 5;         public long a272 = 5;           public long a273 = 5;
            public long a37 = 5;         public long a371 = 5;         public long a372 = 5;           public long a373 = 5;
            public long a47 = 5;         public long a471 = 5;         public long a472 = 5;           public long a473 = 5;
            public long a57 = 5;         public long a571 = 5;         public long a572 = 5;           public long a573 = 5;
            public long a67 = 5;         public long a671 = 5;         public long a672 = 5;           public long a673 = 5;
            public long a77 = 5;         public long a771 = 5;         public long a772 = 5;           public long a773 = 5;
            public long a87 = 5;         public long a871 = 5;         public long a872 = 5;           public long a873 = 5;
            public long a97 = 5;         public long a971 = 5;         public long a972 = 5;           public long a973 = 5;
            public long a18 = 5;         public long a181 = 5;         public long a182 = 5;           public long a183 = 5;
            public long a28 = 5;         public long a281 = 5;         public long a282 = 5;           public long a283 = 5;
            public long a38 = 5;         public long a381 = 5;         public long a382 = 5;           public long a383 = 5;
            public long a48 = 5;         public long a481 = 5;         public long a482 = 5;           public long a483 = 5;
            public long a58 = 5;         public long a581 = 5;         public long a582 = 5;           public long a583 = 5;
            public long a68 = 5;         public long a681 = 5;         public long a682 = 5;           public long a683 = 5;
            public long a78 = 5;         public long a781 = 5;         public long a782 = 5;           public long a783 = 5;
            public long a88 = 5;         public long a881 = 5;         public long a882 = 5;           public long a883 = 5;
            public long a98 = 5;         public long a981 = 5;         public long a982 = 5;           public long a983 = 5;
            public long a19 = 5;         public long a191 = 5;         public long a192 = 5;           public long a193 = 5;
            public long a29 = 5;         public long a291 = 5;         public long a292 = 5;           public long a293 = 5;
            public long a39 = 5;         public long a391 = 5;         public long a392 = 5;           public long a393 = 5;
            public long a49 = 5;         public long a491 = 5;         public long a492 = 5;           public long a493 = 5;
            public long a59 = 5;         public long a591 = 5;         public long a592 = 5;           public long a593 = 5;
            public long a69 = 5;         public long a691 = 5;         public long a692 = 5;           public long a693 = 5;
            public long a79 = 5;         public long a791 = 5;         public long a792 = 5;           public long a793 = 5;
            public long a89 = 5;         public long a891 = 5;         public long a892 = 5;           public long a893 = 5;
            public long a99 = 5;         public long a991 = 5;         public long a992 = 5;           public long a993 = 5;
            public long a10 = 5;         public long a101 = 5;         public long a102 = 5;           public long a103 = 5;
            public long a20 = 5;         public long a201 = 5;         public long a202 = 5;           public long a203 = 5;
            public long a30 = 5;         public long a301 = 5;         public long a302 = 5;           public long a303 = 5;
            public long a40 = 5;         public long a401 = 5;         public long a402 = 5;           public long a403 = 5;
            public long a50 = 5;         public long a501 = 5;         public long a502 = 5;           public long a503 = 5;
            public long a60 = 5;         public long a601 = 5;         public long a602 = 5;           public long a603 = 5;
            public long a70 = 5;         public long a701 = 5;         public long a702 = 5;           public long a703 = 5;
            public long a80 = 5;         public long a801 = 5;         public long a802 = 5;           public long a803 = 5;
            public long a90 = 5;         public long a901 = 5;         public long a902 = 5;           public long a903 = 5;
        }
    }
}