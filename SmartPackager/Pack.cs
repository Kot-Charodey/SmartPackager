using System;

namespace SmartPackager
{
    /// <summary>
    /// defines the types of variables for packaging (there must be implementations of packaging of these types)
    /// </summary>
    public class Pack
    {
        internal IPackagerMethodGeneric[] PackFunction;

        /// <summary>
        /// Searches for or tries to create a packing method for the specified type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IPackagerMethodGeneric GetMethods<T>()
        {
            if (typeof(T).IsUnManaged())
            {
                return BasicPackMethods.PackStructUnmanagedExtension.Make<T>();
            }
            else
            {
                //searches for a method implementation for this type or tries to generate
                IPackagerMethodGeneric ipm;
                if (PackMethods.PackMethodsDictionary.TryGetValue(typeof(T), out ipm))
                {
                    return ipm;
                }
                return BasicPackMethods.Managed.PackStructManagedAutomaticExtension.Make<T>();
            }
        }

        #region Create...
        public static Pack Create<T1>()
        {
            Pack pack = new Pack()
            {
                PackFunction = new IPackagerMethodGeneric[] {
                    GetMethods<T1>()
                }
            };

            return pack;
        }

        public static Pack Create<T1, T2>()
        {
            Pack pack = new Pack()
            {
                PackFunction = new IPackagerMethodGeneric[] {
                    GetMethods<T1>(),
                    GetMethods<T2>()

                }
            };
            return pack;
        }

        public static Pack Create<T1, T2, T3>()
        {
            Pack pack = new Pack()
            {
                PackFunction = new IPackagerMethodGeneric[] {
                    GetMethods<T1>(),
                    GetMethods<T2>(),
                    GetMethods<T3>(),

                }
            };
            return pack;
        }

        public static Pack Create<T1, T2, T3, T4>()
        {
            Pack pack = new Pack()
            {
                PackFunction = new IPackagerMethodGeneric[] {
                    GetMethods<T1>(),
                    GetMethods<T2>(),
                    GetMethods<T3>(),
                    GetMethods<T4>()

                }
            };
            return pack;
        }

        public static Pack Create<T1, T2, T3, T4, T5>()
        {
            Pack pack = new Pack()
            {
                PackFunction = new IPackagerMethodGeneric[] {
                    GetMethods<T1>(),
                    GetMethods<T2>(),
                    GetMethods<T3>(),
                    GetMethods<T4>(),
                    GetMethods<T5>()

                }
            };
            return pack;
        }

        public static Pack Create<T1, T2, T3, T4, T5, T6>()
        {
            Pack pack = new Pack()
            {
                PackFunction = new IPackagerMethodGeneric[] {
                    GetMethods<T1>(),
                    GetMethods<T2>(),
                    GetMethods<T3>(),
                    GetMethods<T4>(),
                    GetMethods<T5>(),
                    GetMethods<T6>()

                }
            };
            return pack;
        }

        public static Pack Create<T1, T2, T3, T4, T5, T6, T7>()
        {
            Pack pack = new Pack()
            {
                PackFunction = new IPackagerMethodGeneric[] {
                    GetMethods<T1>(),
                    GetMethods<T2>(),
                    GetMethods<T3>(),
                    GetMethods<T4>(),
                    GetMethods<T5>(),
                    GetMethods<T6>(),
                    GetMethods<T7>()

                }
            };
            return pack;
        }

        public static Pack Create<T1, T2, T3, T4, T5, T6, T7, T8>()
        {
            Pack pack = new Pack()
            {
                PackFunction = new IPackagerMethodGeneric[] {
                    GetMethods<T1>(),
                    GetMethods<T2>(),
                    GetMethods<T3>(),
                    GetMethods<T4>(),
                    GetMethods<T5>(),
                    GetMethods<T6>(),
                    GetMethods<T7>(),
                    GetMethods<T8>()

                }
            };
            return pack;
        }

        public static Pack Create<T1, T2, T3, T4, T5, T6, T7, T8, T9>()
        {
            Pack pack = new Pack()
            {
                PackFunction = new IPackagerMethodGeneric[] {
                    GetMethods<T1>(),
                    GetMethods<T2>(),
                    GetMethods<T3>(),
                    GetMethods<T4>(),
                    GetMethods<T5>(),
                    GetMethods<T6>(),
                    GetMethods<T7>(),
                    GetMethods<T8>(),
                    GetMethods<T9>()

                }
            };
            return pack;
        }

        public static Pack Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>()
        {
            Pack pack = new Pack()
            {
                PackFunction = new IPackagerMethodGeneric[] {
                    GetMethods<T1>(),
                    GetMethods<T2>(),
                    GetMethods<T3>(),
                    GetMethods<T4>(),
                    GetMethods<T5>(),
                    GetMethods<T6>(),
                    GetMethods<T7>(),
                    GetMethods<T8>(),
                    GetMethods<T9>(),
                    GetMethods<T10>()

                }
            };
            return pack;
        }

        public static Pack Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>()
        {
            Pack pack = new Pack()
            {
                PackFunction = new IPackagerMethodGeneric[] {
                    GetMethods<T1>(),
                    GetMethods<T2>(),
                    GetMethods<T3>(),
                    GetMethods<T4>(),
                    GetMethods<T5>(),
                    GetMethods<T6>(),
                    GetMethods<T7>(),
                    GetMethods<T8>(),
                    GetMethods<T9>(),
                    GetMethods<T10>(),
                    GetMethods<T11>()

                }
            };
            return pack;
        }

        public static Pack Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>()
        {
            Pack pack = new Pack()
            {
                PackFunction = new IPackagerMethodGeneric[] {
                    GetMethods<T1>(),
                    GetMethods<T2>(),
                    GetMethods<T3>(),
                    GetMethods<T4>(),
                    GetMethods<T5>(),
                    GetMethods<T6>(),
                    GetMethods<T7>(),
                    GetMethods<T8>(),
                    GetMethods<T9>(),
                    GetMethods<T10>(),
                    GetMethods<T11>(),
                    GetMethods<T12>()

                }
            };
            return pack;
        }

        public static Pack Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>()
        {
            Pack pack = new Pack()
            {
                PackFunction = new IPackagerMethodGeneric[] {
                    GetMethods<T1>(),
                    GetMethods<T2>(),
                    GetMethods<T3>(),
                    GetMethods<T4>(),
                    GetMethods<T5>(),
                    GetMethods<T6>(),
                    GetMethods<T7>(),
                    GetMethods<T8>(),
                    GetMethods<T9>(),
                    GetMethods<T10>(),
                    GetMethods<T11>(),
                    GetMethods<T12>(),
                    GetMethods<T13>()

                }
            };
            return pack;
        }

        public static Pack Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>()
        {
            Pack pack = new Pack()
            {
                PackFunction = new IPackagerMethodGeneric[] {
                    GetMethods<T1>(),
                    GetMethods<T2>(),
                    GetMethods<T3>(),
                    GetMethods<T4>(),
                    GetMethods<T5>(),
                    GetMethods<T6>(),
                    GetMethods<T7>(),
                    GetMethods<T8>(),
                    GetMethods<T9>(),
                    GetMethods<T10>(),
                    GetMethods<T11>(),
                    GetMethods<T12>(),
                    GetMethods<T13>(),
                    GetMethods<T14>()
                }
            };
            return pack;
        }

        public static Pack Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>()
        {
            Pack pack = new Pack()
            {
                PackFunction = new IPackagerMethodGeneric[] {
                    GetMethods<T1>(),
                    GetMethods<T2>(),
                    GetMethods<T3>(),
                    GetMethods<T4>(),
                    GetMethods<T5>(),
                    GetMethods<T6>(),
                    GetMethods<T7>(),
                    GetMethods<T8>(),
                    GetMethods<T9>(),
                    GetMethods<T10>(),
                    GetMethods<T11>(),
                    GetMethods<T12>(),
                    GetMethods<T13>(),
                    GetMethods<T14>(),
                    GetMethods<T15>()

                }
            };
            return pack;
        }

        public static Pack Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>()
        {
            Pack pack = new Pack()
            {
                PackFunction = new IPackagerMethodGeneric[] {
                    GetMethods<T1>(),
                    GetMethods<T2>(),
                    GetMethods<T3>(),
                    GetMethods<T4>(),
                    GetMethods<T5>(),
                    GetMethods<T6>(),
                    GetMethods<T7>(),
                    GetMethods<T8>(),
                    GetMethods<T9>(),
                    GetMethods<T10>(),
                    GetMethods<T11>(),
                    GetMethods<T12>(),
                    GetMethods<T13>(),
                    GetMethods<T14>(),
                    GetMethods<T15>(),
                    GetMethods<T16>()

                }
            };
            return pack;
        }

        public static Pack Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>()
        {
            Pack pack = new Pack()
            {
                PackFunction = new IPackagerMethodGeneric[] {
                    GetMethods<T1>(),
                    GetMethods<T2>(),
                    GetMethods<T3>(),
                    GetMethods<T4>(),
                    GetMethods<T5>(),
                    GetMethods<T6>(),
                    GetMethods<T7>(),
                    GetMethods<T8>(),
                    GetMethods<T9>(),
                    GetMethods<T10>(),
                    GetMethods<T11>(),
                    GetMethods<T12>(),
                    GetMethods<T13>(),
                    GetMethods<T14>(),
                    GetMethods<T15>(),
                    GetMethods<T16>(),
                    GetMethods<T17>()

                }
            };
            return pack;
        }

        public static Pack Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>()
        {
            Pack pack = new Pack()
            {
                PackFunction = new IPackagerMethodGeneric[] {
                    GetMethods<T1>(),
                    GetMethods<T2>(),
                    GetMethods<T3>(),
                    GetMethods<T4>(),
                    GetMethods<T5>(),
                    GetMethods<T6>(),
                    GetMethods<T7>(),
                    GetMethods<T8>(),
                    GetMethods<T9>(),
                    GetMethods<T10>(),
                    GetMethods<T11>(),
                    GetMethods<T12>(),
                    GetMethods<T13>(),
                    GetMethods<T14>(),
                    GetMethods<T15>(),
                    GetMethods<T16>(),
                    GetMethods<T17>(),
                    GetMethods<T18>()

                }
            };
            return pack;
        }

        public static Pack Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>()
        {
            Pack pack = new Pack()
            {
                PackFunction = new IPackagerMethodGeneric[] {
                    GetMethods<T1>(),
                    GetMethods<T2>(),
                    GetMethods<T3>(),
                    GetMethods<T4>(),
                    GetMethods<T5>(),
                    GetMethods<T6>(),
                    GetMethods<T7>(),
                    GetMethods<T8>(),
                    GetMethods<T9>(),
                    GetMethods<T10>(),
                    GetMethods<T11>(),
                    GetMethods<T12>(),
                    GetMethods<T13>(),
                    GetMethods<T14>(),
                    GetMethods<T15>(),
                    GetMethods<T16>(),
                    GetMethods<T17>(),
                    GetMethods<T18>(),
                    GetMethods<T19>()

                }
            };
            return pack;
        }

        public static Pack Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20>()
        {
            Pack pack = new Pack()
            {
                PackFunction = new IPackagerMethodGeneric[] {
                    GetMethods<T1>(),
                    GetMethods<T2>(),
                    GetMethods<T3>(),
                    GetMethods<T4>(),
                    GetMethods<T5>(),
                    GetMethods<T6>(),
                    GetMethods<T7>(),
                    GetMethods<T8>(),
                    GetMethods<T9>(),
                    GetMethods<T10>(),
                    GetMethods<T11>(),
                    GetMethods<T12>(),
                    GetMethods<T13>(),
                    GetMethods<T14>(),
                    GetMethods<T15>(),
                    GetMethods<T16>(),
                    GetMethods<T17>(),
                    GetMethods<T18>(),
                    GetMethods<T19>(),
                    GetMethods<T20>()

                }
            };
            return pack;
        }
        #endregion
    }
}
