using SmartPackager;

var pack = Packager.Create<int[]>();
int[] kk = new int[10];
for(int i = 0; i < 10; i++)
{
    kk[i] = i;
}
var bytes = pack.PackUP(kk);
pack.UnPack(bytes, 0, out kk);
Console.WriteLine(bytes);
