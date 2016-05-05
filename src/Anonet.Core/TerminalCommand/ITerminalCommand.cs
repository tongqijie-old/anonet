using System;

namespace Anonet.Core
{
    interface ITerminalCommand
    {
        /// <summary>
        /// 终端命令行
        /// </summary>
        TerminalCommandLine TerminalCommandLine { get; }

        /// <summary>
        /// 指示终端命令执行是否需要等待
        /// </summary>
        bool Wait { get; }

        /// <summary>
        /// 终端命令执行结果
        /// </summary>
        object Result { get; }

        /// <summary>
        /// 指示终端命令是否已被处理
        /// </summary>
        bool Handled { get; }

        /// <summary>
        /// 执行终端命令
        /// </summary>
        void Execute(ITerminalCommandChannel terminalCommandChannel, Action<string> prompt);
    }
}