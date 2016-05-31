using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;

namespace pingtracker
{
    public class Program
    {
        private static List<IPAddress> ipAddressList = new List<IPAddress>();

        public static void Main(string[] args)
        {
            int ip_number = 0;

            Writer.createFile();

            Writer.printToConsoleAndFile("", 5, true);
            Writer.printToConsoleAndFile("                   |                                      |                  ", 5, true);
            Writer.printToConsoleAndFile("                   |                                      |                  ", 5, true);
            Writer.printToConsoleAndFile("                |  |  |                                |  |  |               ", 5, true);
            Writer.printToConsoleAndFile("                |  |  |                                |  |  |               ", 5, true);
            Writer.printToConsoleAndFile("             |  |  |  |  |                          |  |  |  |  |            ", 5, true);
            Writer.printToConsoleAndFile("          |  |  |  |  |  |  |                    |  |  |  |  |  |  |         ", 5, true);
            Writer.printToConsoleAndFile("       |  |  |  |  |  |  |  |  |              |  |  |  |  |  |  |  |  |      ", 5, true);
            Writer.printToConsoleAndFile(" |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |", 5, true);
            Writer.printToConsoleAndFile("                             ____ ___ ____   ____ ___  ", 5, true);
            Writer.printToConsoleAndFile("                            / ___|_ _/ ___| / ___/ _ \\ ", 5, true);
            Writer.printToConsoleAndFile("                           | |    | |\\___ \\| |  | | | |", 5, true);
            Writer.printToConsoleAndFile("                           | |___ | | ___) | |__| |_| |", 5, true);
            Writer.printToConsoleAndFile("                            \\____|___|____/ \\____\\___/ ", 5, true);
            Writer.printToConsoleAndFile("", 5, true);
            Writer.printToConsoleAndFile("|=============================================================================|", 5, true);
            Writer.printToConsoleAndFile("|                     pingtracker_v0.3 by nrees@cisco.com                     |", 5, true);
            Writer.printToConsoleAndFile("|-----------------------------------------------------------------------------|", 5, true);
            Writer.printToConsoleAndFile("| This tool lets you ping hosts and informs you:                              |", 5, true);
            Writer.printToConsoleAndFile("|    a.) if pings are successful once or continuously for < 10s (dark green)  |", 5, true);
            Writer.printToConsoleAndFile("|    b.) if pings are successful continuously for >= 10 seconds (light green) |", 5, true);
            Writer.printToConsoleAndFile("|    c.) if pings fail once (yellow)                                          |", 5, true);
            Writer.printToConsoleAndFile("|    d.) if pings fail for more than 15 seconds (red)                         |", 5, true);
            Writer.printToConsoleAndFile("| Enter the IP of the host(s) you wish to ping, then \"start\" to run the tool. |", 5, true);
            Writer.printToConsoleAndFile("|-----------------------------------------------------------------------------|", 5, true);
            Writer.printToConsoleAndFile("| (A log file will be created in the directory of the program automatically)  |", 5, true);
            Writer.printToConsoleAndFile("|=============================================================================|", 5, true);
            Writer.printToConsoleAndFile("", 5, true);
            Writer.printToConsoleAndFile("", 5, true);

            while (true)
            {
                Writer.printToConsoleAndFile("Please enter IP#" + ip_number + ", or \"start\": ", 5, false);
                string input = Console.ReadLine();
                Writer.printToFile(input, true);

                IPAddress ipToPing;
                bool ipIsValid = IPAddress.TryParse(input, out ipToPing);

                if (ipIsValid)
                {
                    ipAddressList.Add(ipToPing);
                    ip_number++;
                }
                else if (!ipIsValid && input.ToUpper().CompareTo("START") == 0)
                {
                    if (ipAddressList.Count == 0)
                    {
                        Writer.printToConsoleAndFile("No IP Addresses entered...", 2, true);
                        continue;
                    }
                    start();
                    break;
                }
                else if (!ipIsValid && (input.ToUpper().CompareTo("EXIT") == 0) || (input.ToUpper().CompareTo("QUIT") == 0))
                {
                    Writer.printToConsoleAndFile("EXITING...", 2, true);
                    Thread.Sleep(3000);
                    return;
                }
                else if (!ipIsValid && input.CompareTo(String.Empty) == 0)
                    continue;
                else
                {
                    Writer.printToConsoleAndFile("Please enter a valid IPv4 Address.", 2, true);
                }
            }
        }

        private static void start()
        {
            foreach (IPAddress ip in ipAddressList)
            {
                TrackedPing ping = new TrackedPing(ip);
                Thread thread = new Thread(() => ping.startPingLoop());
                thread.Start();
            }
        }
    }
}