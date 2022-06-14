using System;
using System.Collections;
using System.Runtime.CompilerServices;

namespace SmartPackager.ByteStack
{
    /// <summary>
    /// Считывает данные из буфера
    /// </summary>
    public struct ByteReader
    {
        /// <summary>
        /// Статус потока
        /// </summary>
        public enum StreamState
        {
            /// <summary>
            ///
            /// </summary>
            Default,
            /// <summary>
            /// Ожидается вызов функции AttachReference
            /// </summary>
            WaitAttach,
            /// <summary>
            /// Ожидается вызов 
            /// </summary>
            WaitGetObject
        }

        private readonly UnsafeArray UnsafeArray;
        private readonly RefArray RefArray;
        /// <summary>
        /// Статус потока (ожидаймая следуйщая инструкция)
        /// </summary>
        public StreamState State { get; private set; }
        private int Pos;
        private RefPoint Point_;
        private object GetObject_;

        internal ByteReader(UnsafeArray unsafeArray):this()
        {
            UnsafeArray = unsafeArray;
            Pos = 0;
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void CheckState(StreamState state)
        {
            if (State != state)
            {
                throw new Exception("Ожидалась другая инструкция!");
            }
        }

        /// <summary>
        /// Считать значение из буфера
        /// </summary>
        /// <typeparam name="T">тип значения</typeparam>
        /// <returns>считаное значение</returns>
        public unsafe T Read<T>() where T : unmanaged
        {
            CheckState(StreamState.Default);
            var data = UnsafeArray.Get<T>(Pos);
            Pos += sizeof(T);
            return data;
        }

        public unsafe T[] Read<T>(int length) where T : unmanaged
        {
            CheckState(StreamState.Default);
            var data = UnsafeArray.Get<T>(Pos, length);
            Pos += sizeof(T);
            return data;
        }

        public int ReadLength()
        {
            CheckState(StreamState.Default);
            var len = UnsafeArray.Get<int>(Pos);
            Pos += sizeof(int);
            return len;
        }

        public bool ReadExists()
        {
            CheckState(StreamState.Default);
            var exists = UnsafeArray.Get<bool>(Pos);
            Pos += sizeof(bool);
            return exists;
        }

        /// <summary>
        /// Считывает ссылку
        /// </summary>
        /// <returns>если true то необходимо получить объект (GetReferenceObject) иначе создать ссылку и объект (AttachReference)</returns>
        public bool ReadReference()
        {
            CheckState(StreamState.Default);
            int pos = UnsafeArray.Get<int>(Pos);
            Pos += sizeof(bool);
            if (pos == RefPoint.NULL)
            {
                State = StreamState.WaitGetObject;
                GetObject_ = null;
                return true;
            }
            else if(pos == RefPoint.DATA)
            {
                State = StreamState.WaitAttach;
                Point_ = new RefPoint(pos, null);
                return false;
            }
            else
            {
                State = StreamState.WaitGetObject;
                GetObject_ = RefArray.GetObject(pos);
                return true;
            }
        }

        public void AttachReference(object val)
        {
            CheckState(StreamState.WaitAttach);
            State = StreamState.Default;
            Point_.Data = val;
            RefArray.AddRef(Point_);
        }

        public object GetReferenceObject()
        {
            CheckState(StreamState.WaitGetObject);
            State = StreamState.Default;
            return GetObject_;
        }
    }
}