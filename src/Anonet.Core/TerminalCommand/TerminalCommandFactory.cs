using System.Reflection;
using System.Linq;

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

            var terminalCommand = Assembly.GetExecutingAssembly().GetTypes().FirstOrDefault(
                x => x.GetCustomAttribute<TerminalCommandAttribute>(false) != null 
                    && x.GetCustomAttribute<TerminalCommandAttribute>(false).SupportedCommandCodes.Contains(terminalCommandLine.CommandCode));
            if (terminalCommand == null)
            {
                return null;
            }

            return terminalCommand as ITerminalCommand;
        }
    }
}
