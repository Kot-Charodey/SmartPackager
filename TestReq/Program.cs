﻿//using SmartPackager;

//var pack = Packager.Create<N1>();
//N1 b = new N1();
//b.a = 33;
//b.n1 = new N2() { a = 55 };
//b.n2 = b.n1;
//var bytes = pack.PackUP(b);
//pack.UnPack(bytes, 0, out b);
//Console.WriteLine(bytes);
//
//class N1
//{
//    public int a;
//    public N2 n1;
//    public N2 n2;
//}
//
//class N2
//{
//    public int a;
//}


//var pack = Packager.Create<N1[]>();
//N1[] b = new N1[2];
//b[0] = new N1() { a= 99 };
//b[1] = new N1() { a = 99 };
//b[0].n = new N2() { a = 15 };
//b[1].n = b[0].n;
//var bytes = pack.PackUP(b);
//pack.UnPack(bytes, 0, out b);
//Console.WriteLine(bytes);
//
//class N1
//{
//    public int a;
//    public N2 n;
//}
//
//class N2
//{
//    public int a;
//}