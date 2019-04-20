using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public enum Info { Error = 0, Info = 1, Warning = 3, Normal = 4 }
    class cmd
    {
        

        public static void WriteLine(string Text)
        {
            WriteLine(Text, Info.Normal);
        }

        public static void WriteLine(string Text,Info LogType)
        {
            switch (LogType)
            {
                case Info.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    TextWriter err = new TextWriter(AppDomain.CurrentDomain.BaseDirectory, "error_" + DateTime.Now.ToString("MMddyyyy") + ".log", "Logs", false);

                    err.WriteLine(DateTime.Now.ToString() +"-"+ Text);
                    err.Close();
                    Console.WriteLine(Text);
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case Info.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine(Text);
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case Info.Normal:
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(Text);
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case Info.Info:
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(Text);
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
            }
        }
    }
}
