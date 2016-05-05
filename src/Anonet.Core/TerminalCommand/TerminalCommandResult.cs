namespace Anonet.Core
{
    class TerminalCommandResult
    {
        public static object Done()
        {
            return "done.";
        }

        public static object Error(string info)
        {
            return info;
        }

        public static object InvalidArguments()
        {
            return "invalid arguments exist.";
        }
    }
}
