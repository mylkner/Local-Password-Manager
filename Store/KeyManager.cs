namespace Store
{
    public static class KeyManager
    {
        private static byte[]? _key;

        public static void SetKey(byte[] key)
        {
            if (_key == null)
            {
                _key = key;
            }
            else
            {
                throw new InvalidOperationException("Key has been set.");
            }
        }

        public static byte[] GetKey()
        {
            if (_key == null)
            {
                throw new InvalidOperationException("Key has not been set.");
            }
            return _key;
        }
    }
}
