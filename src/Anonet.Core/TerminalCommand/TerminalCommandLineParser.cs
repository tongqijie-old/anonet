namespace Anonet.Core
{
    class TerminalCommandLineParser
    {
        public static TerminalCommandLine Parse(string terminalCommandText)
        {
            terminalCommandText = terminalCommandText.Trim();

            if (string.IsNullOrEmpty(terminalCommandText))
            {
                return null;
            }

            var current = terminalCommandText.IndexOf(' ');
            if (current == -1)
            {
                return new TerminalCommandLine(terminalCommandText, terminalCommandText);
            }

            var terminalCommandLine = new TerminalCommandLine(terminalCommandText, terminalCommandText.Substring(0, current));

            string key = null, value = null;
            for (var i = current; i < terminalCommandText.Length; )
            {
                if (terminalCommandText[i] == ' ')
                {
                    i++;
                    continue;
                }
                else if (terminalCommandText[i] == '-')
                {
                    if (i == terminalCommandText.Length - 1)
                    {
                        return null;
                    }

                    var end = terminalCommandText.IndexOf(' ', i);
                    if (end == -1)
                    {
                        return null;
                    }
                    else
                    {
                        key = terminalCommandText.Substring(i + 1, end - i - 1);
                        i = end + 1;
                    }
                }
                else if (terminalCommandText[i] == '"')
                {
                    var end = terminalCommandText.IndexOf('"', i + 1);
                    if (end == -1)
                    {
                        return null;
                    }

                    value = terminalCommandText.Substring(i + 1, end - i - 1).Trim();
                    i = end + 1;

                    if (key == null)
                    {
                        return null;
                    }

                    terminalCommandLine.AddArg(key, value);

                    key = null;
                    value = null;
                }
                else
                {
                    var end = terminalCommandText.IndexOf(' ', i);
                    if (end == -1)
                    {
                        value = terminalCommandText.Substring(i).Trim();
                        i = terminalCommandText.Length;
                    }
                    else
                    {
                        value = terminalCommandText.Substring(i, end - i).Trim();
                        i = end + 1;
                    }

                    if (key == null)
                    {
                        return null;
                    }

                    terminalCommandLine.AddArg(key, value);

                    key = null;
                    value = null;
                }
            }

            return terminalCommandLine;
        }
    }
}
