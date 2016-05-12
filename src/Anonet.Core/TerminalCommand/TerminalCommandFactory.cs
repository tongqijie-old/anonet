using System.Reflection;
using System.Linq;
using System;

namespace Anonet.Core
{
    class TerminalCommandFactory
    {
        public static ITerminalCommand Create(string terminalCommandText)
        {
            var terminalCommandLine = TerminalCommandLineParser.Parse(terminalCommandText);
            if (terminalCommandLine == null)
            {
                return null;
            }

            var terminalCommandType = Assembly.GetExecutingAssembly().GetTypes().FirstOrDefault(
                x => x.GetCustomAttribute<TerminalCommandAttribute>(false) != null 
                    && x.GetCustomAttribute<TerminalCommandAttribute>(false).SupportedCommandCodes.Contains(terminalCommandLine.CommandCode));
            if (terminalCommandType == null)
            {
                return null;
            }

            return Activator.CreateInstance(terminalCommandType, terminalCommandLine) as ITerminalCommand;
        }
    }
}
