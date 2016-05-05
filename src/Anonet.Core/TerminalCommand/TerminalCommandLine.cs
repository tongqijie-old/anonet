using System.Collections.Generic;
using System.Linq;

namespace Anonet.Core
{
    class TerminalCommandLine
    {
        public TerminalCommandLine(string completeCommandText, string commandCode)
        {
            CompleteCommandText = completeCommandText;
            CommandCode = commandCode;
            Arguments = new Dictionary<string, string>();
        }

        public string CommandCode { get; private set; }

        private Dictionary<string, string> Arguments { get; set; }

        public string CompleteCommandText { get; private set; }

        public void AddArg(string name, string value)
        {
            Arguments[name] = value;
        }

        public void AddArgs(Dictionary<string, string> args)
        {
            foreach (var kv in args)
            {
                AddArg(kv.Key, kv.Value);
            }
        }

        public bool ContainKeys(params string[] keys)
        {
            return keys.All(x => Arguments.ContainsKey(x));
        }

        public string[] Keys
        {
            get
            {
                return Arguments.Keys.ToArray();
            }
        }

        public string this[string name]
        {
            get
            {
                if (Arguments.ContainsKey(name))
                {
                    return Arguments[name];
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
