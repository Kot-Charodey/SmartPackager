public unsafe struct UInt24
{
    private fixed byte number[3];

    public const uint MaxValue = 0xffffff;
    public const uint MinValue = 0x000000;

    private UInt24(byte a, byte b, byte c)
    {
        number[0] = a;
        number[1] = b;
        number[2] = c;
    }

    public override string ToString()
    {
        uint num = 0;
        byte* t = (byte*)(&num);
        t[0] = number[0];
        t[1] = number[1];
        t[2] = number[2];

        return num.ToString();
    }

    private static byte FromBool(bool b)
    {
        if (b)
            return 1;
        else
            return 0;
    }

    public override bool Equals(object obj)
    {
        if (obj is UInt24)
        {
            UInt24 ui = (UInt24)obj;
            return number[0] == ui.number[0] && number[1] == ui.number[1] && number[2] == ui.number[2];
        }
        else
        {
            return false;
        }
    }

    public override int GetHashCode()
    {
        return (int)(uint)this;
    }

    public static UInt24 operator +(UInt24 a, UInt24 b)
    {
        UInt24 c = new UInt24();

        int tmp = a.number[0] + b.number[0];
        c.number[0] = (byte)(tmp);

        tmp = a.number[1] + b.number[1] + FromBool(tmp > byte.MaxValue);
        c.number[1] = (byte)(tmp);

        tmp = a.number[2] + b.number[2] + FromBool(tmp > byte.MaxValue);
        c.number[2] = (byte)(tmp);

        return c;
    }

    public static UInt24 operator -(UInt24 a, UInt24 b)
    {
        UInt24 c = new UInt24();

        uint tmp = a.number[0] - (uint)b.number[0];
        c.number[0] = (byte)(tmp);

        tmp = a.number[1] - (uint)b.number[1] - FromBool(tmp > byte.MaxValue);
        c.number[1] = (byte)(tmp);

        tmp = a.number[2] - (uint)b.number[2] - FromBool(tmp > byte.MaxValue);
        c.number[2] = (byte)(tmp);

        return c;
    }

    public static bool operator ==(UInt24 a, UInt24 b)
    {
        return a.number[0] == b.number[0] && a.number[1] == b.number[1] && a.number[2] == b.number[2];
    }

    public static bool operator !=(UInt24 a, UInt24 b)
    {
        return a.number[0] != b.number[0] && a.number[1] != b.number[1] && a.number[2] != b.number[2];
    }

    public static implicit operator UInt24(int a)
    {
        return (uint)a;
    }

    public static implicit operator UInt24(uint a)
    {
        byte* p = (byte*)&a;
        return new UInt24(p[0], p[1], p[2]);
    }

    public static explicit operator uint(UInt24 a)
    {
        uint num = 0;
        byte* t = (byte*)(&num);
        t[0] = a.number[0];
        t[1] = a.number[1];
        t[2] = a.number[2];

        return num;
    }

    public static implicit operator string(UInt24 a)
    {
        return a.ToString();
    }
}