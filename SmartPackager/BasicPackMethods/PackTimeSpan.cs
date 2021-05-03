﻿using System;

namespace SmartPackager.BasicPackMethods
{
    internal class PackTimeSpan : IPackagerMethod<TimeSpan>
    {
        public Type TargetType => typeof(TimeSpan);

        public bool IsFixedSize => true;

        public unsafe long PackUP(byte* destination, TimeSpan source)
        {
            *(long*)destination = source.Ticks;
            return sizeof(long);
        }

        public unsafe long UnPack(byte* source, out TimeSpan destination)
        {
            TimeSpan pt = new TimeSpan(*(long*)source);
            destination = pt;
            return sizeof(long);
        }

        public long GetSize(TimeSpan source)
        {
            return sizeof(long);
        }
    }
}