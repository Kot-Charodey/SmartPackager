namespace SmartPackager.Collections.Generic
{
    public static class Dll
    {
        private static bool init = false;
        /// <summary>
        /// Костыль которая заставляет компилятор включить данную dll в сборку + подключает к SmartPackager
        /// </summary>
        /// <returns></returns>
        public static unsafe void Plug(bool fake=true)
        {
            if (fake)
                init = true;
            if (!init)
            {
                init = true;
                PackMethods.SetupAgainPackMethods();
            }
        }
    }
}
