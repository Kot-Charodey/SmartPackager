using SmartPackager;

var pack = Packager.Create<N1[]>();
N1[] b = new N1[2];
b[0] = new N1() { a= 99 };
b[1] = new N1() { a = 99 };
b[0].n = new N2() { a = 15 };
b[1].n = b[0].n;
var bytes = pack.PackUP(b);
pack.UnPack(bytes, 0, out b);
Console.WriteLine(bytes);

class N1
{
    public int a;
    public N2 n;
}

class N2
{
    public int a;
}