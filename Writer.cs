using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pingtracker
{
    static class Writer
    {
        private static String day = DateTime.Now.Date.Day.ToString();
        private static String month = DateTime.Now.Date.Month.ToString();
        private static String year = DateTime.Now.Date.Year.ToString();
        private static String second = DateTime.Now.Second.ToString();
        private static String minute = DateTime.Now.Minute.ToString();
        private static String hour = DateTime.Now.Hour.ToString();
        private static String filenamepostfix = day + "." + month + "." + year + "_" + hour + "" + minute + "" + second;

        private static StreamWriter file;

        private static readonly object ConsoleWriterLock = new object();

        public static void createFile()
        {
            file = new StreamWriter("./pingtracker_log_" + filenamepostfix + ".txt");
            file.AutoFlush = true;
        }

        public static void printToConsoleAndFile(string text, int severity, bool newLine)
        {
            printToConsole(text, severity, newLine);
            printToFile(text, newLine);
        }

        public static void printToConsole(string text, int severity, bool newLine) 
        {
            lock (ConsoleWriterLock)
            {
                setConsoleColor(severity);
               
                if (newLine)
                    Console.WriteLine(text);
                else
                    Console.Write(text);

                resetConsoleColor();
            }
        }

        public static void printToFile(string text, bool newLine)
        {
            if (newLine)
                file.WriteLine(text);
            else
                file.Write(text);
        }

        private static void setConsoleColor(int severity)
        {
            switch (severity)
            {
                //exception
                case 0:
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    break;
                //alert
                case 1:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                //warning
                case 2:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                //success once
                case 3:
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    break;
                //continuous success
                case 4:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                //informational
                case 5:
                    resetConsoleColor();
                    break;
                //default
                default:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
            }
        }

        private static void resetConsoleColor()
        {
            Console.ResetColor();
        }
    }
}