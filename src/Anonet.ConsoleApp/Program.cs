using System;

namespace Anonet.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var app = new Core.App();

            string command = null;
            Console.Write("anonet: ");
            while ((command = Console.ReadLine()) != "quit")
            {
                app.Execute(command);
                Console.Write("anonet: ");
            }

            app.Dispose();
        }
    }
}
