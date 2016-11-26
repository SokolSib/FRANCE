namespace TicketWindow.Global
{
    public static class SyncData
    {
        private static bool _isConnect = true;

        static SyncData()
        {
            IsSync = false;
        }

        public static bool IsConnect { get { return _isConnect; } }
        public static bool IsSync { get; private set; }

        public static void SetSunc(bool isSunc )
        {
            IsSync = isSunc;
        }

        public static void SetConnect(bool isConnect)
        {
            _isConnect = isConnect;
        }
    }
}
