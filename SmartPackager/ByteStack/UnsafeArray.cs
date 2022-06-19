using System;
using System.Runtime.CompilerServices;

#if DEBUG
[assembly: InternalsVisibleTo("UnitTest")]
#endif
namespace SmartPackager.ByteStack
{

    /// <see cref="UnsafeArray"/>
    internal delegate void UnsafeArrayAction(ref UnsafeArray array);

    /// <summary>
    /// Позволяет отночительно безопасно работать с регмоном памяти (запись чтение неуправляймых типов)
    /// </summary>
    internal unsafe struct UnsafeArray
    {
        private byte* Buffer;
        private int Length;
        private bool CanUse;

        /// <summary>
        /// Запускает делегат и передаёт туда UnsafeArray (его можно использовать только до окончания работы данного делегата)
        /// </summary>
        /// <param name="array">массив который будет использоваться</param>
        /// <param name="offset">смещение отностительно массива</param>
        /// <param name="length">размер неуправляймого массива</param>
        /// <param name="action">делегат в котором будет доступен данный массив для использования (будет вызван сразу)</param>
        /// <exception cref="ArgumentOutOfRangeException">возникнет при выходи смещения и указанной длинны за границы массива </exception>
        public static void UseArray(byte[] array, int offset, int length, UnsafeArrayAction action)
        {
            lock (array)
            {
                if (array.Length - offset < length)
                    throw new ArgumentOutOfRangeException("SmartPackager => incorrect data");
                fixed (byte* p = array)
                {
                    var uArray = new UnsafeArray
                    {
                        Buffer = p + offset,
                        Length = length,
                        CanUse = true
                    };
                    action(ref uArray);
                    uArray.CanUse = false;
                }
            }
        }

        /// <summary>
        /// Генерирует ошибку если функция была вызвана за пределами делегата вызова
        /// </summary>
        /// <see cref="UseArray(byte[], int, int, UnsafeArrayAction)"/>
        /// <exception cref="InvalidOperationException">массив может использоваться только в функции делегата</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void TrowUse()
        {
            if (!CanUse)
                throw new InvalidOperationException("SmartPackager => an array can only be used in a delegate function!");
        }

        /// <summary>
        /// Генерирует ошибку если индекс вышел за диапазон массива
        /// </summary>
        /// <param name="maxIndex">максимальный необходимый индекс для записи</param>
        /// <exception cref="ArgumentOutOfRangeException">возникнет если массив выйдет за границы буфера</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ThrowLength(int maxIndex)
        {
            if (maxIndex > Length)
                throw new ArgumentOutOfRangeException("index");
        }

        /*
        /// <summary>
        /// Получает указатель на регион буфера
        /// </summary>
        /// <param name="offset">смещение</param>
        /// <returns>указатель на адресс</returns>
        /// <exception cref="ArgumentOutOfRangeException">возникнет если массив выйдет за границы буфера</exception>
        /// <exception cref="InvalidOperationException">функцию можно выполнять только из делегата при создании</exception>
        public byte* Get(int offset)
        {
            TrowUse();
            ThrowLength(offset);

            return Buffer + offset;
        }
        */

        /// <summary>
        /// Извлекает неуправляймый тип из буфера
        /// </summary>
        /// <typeparam name="T">неуправляймый тип</typeparam>
        /// <param name="offset">смещение</param>
        /// <returns>значение</returns>
        /// <exception cref="ArgumentOutOfRangeException">возникнет если массив выйдет за границы буфера</exception>
        /// <exception cref="InvalidOperationException">функцию можно выполнять только из делегата при создании</exception>
        public T Get<T>(int offset) where T : unmanaged
        {
            TrowUse();
            ThrowLength(offset + sizeof(T));

            return *((T*)(Buffer + offset));
        }

        /// <summary>
        /// Записывает неуправляймый тип в буфер
        /// </summary>
        /// <typeparam name="T">неуправляймый тип</typeparam>
        /// <param name="offset">смещение</param>
        /// <param name="val">значение для записи</param>
        /// <exception cref="ArgumentOutOfRangeException">возникнет если массив выйдет за границы буфера</exception>
        /// <exception cref="InvalidOperationException">функцию можно выполнять только из делегата при создании</exception>
        public void Set<T>(int offset, T val) where T : unmanaged
        {
            TrowUse();
            ThrowLength(offset + sizeof(T));

            var ptr = Buffer + offset;

            *((T*)(Buffer + offset)) = val;
        }

        /// <summary>
        /// Записывает неуправляймый массив в буфер
        /// </summary>
        /// <typeparam name="T">тип массива </typeparam>
        /// <param name="offset">смещение</param>
        /// <param name="array">массив</param>
        /// <exception cref="ArgumentOutOfRangeException">возникнет если массив выйдет за границы буфера</exception>
        /// <exception cref="InvalidOperationException">функцию можно выполнять только из делегата при создании</exception>
        public void Set<T>(int offset, T[] array) where T : unmanaged
        {
            TrowUse();
            int len = sizeof(T) * array.Length;
            ThrowLength(offset + len);

            lock (array)
            {
                fixed (void* arr_ptr = array)
                {
                    System.Buffer.MemoryCopy(arr_ptr, Buffer + offset, len, len);
                }
            }
        }

        /// <summary>
        /// Извлекает неуправляймый массив из буфера
        /// </summary>
        /// <typeparam name="T">тип массива</typeparam>
        /// <param name="offset">смещение</param>
        /// <param name="size">размер массива</param>
        /// <returns>массив</returns>
        /// <exception cref="ArgumentOutOfRangeException">возникнет если массив выйдет за границы буфера</exception>
        /// <exception cref="InvalidOperationException">функцию можно выполнять только из делегата при создании</exception>
        public T[] Get<T>(int offset, int size) where T : unmanaged
        {
            TrowUse();
            int len = sizeof(T) * size;
            ThrowLength(offset + len);

            T[] array = new T[size];
            fixed (void* arr_ptr = array)
            {
                System.Buffer.MemoryCopy(Buffer + offset, arr_ptr, len, len);
            }
            return array;
        }
    }
}