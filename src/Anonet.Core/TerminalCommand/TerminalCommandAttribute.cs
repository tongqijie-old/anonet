using System;

namespace Anonet.Core
{
    class TerminalCommandAttribute : Attribute
    {
        public TerminalCommandAttribute(string[] supportedCommandCodes)
        {
            SupportedCommandCodes = supportedCommandCodes;
        }

        public string[] SupportedCommandCodes { get; set; }
    }
}