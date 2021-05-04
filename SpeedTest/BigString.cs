namespace SpeedTest
{
    public static class BigString
    {
        public static string GetString()
        {
            char[] sums = new char[4096 * 2];
            return new string(sums);
        }
    }
}
