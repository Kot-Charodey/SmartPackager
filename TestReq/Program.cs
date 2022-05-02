using SmartPackager;

var pack = Packager.Create<NClass>();
NClass b = new();
b.kk = b;
var bytes = pack.PackUP(b);
pack.UnPack(bytes, 0, out b);
Console.WriteLine(bytes);

class NClass
{
    public NClass kk;
}