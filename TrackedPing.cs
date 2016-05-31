using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pingtracker
{
    public class TrackedPing : Ping
    {

        public TrackedPing(IPAddress ip) : base()
        {
            this.ip = ip;

            failedPings = 0;
            successfulPings = 0;
        }

        public IPAddress ip { get; set; }
        private int failedPings { get; set; }
        private int successfulPings { get; set; }

        public void startPingLoop()
        {
            while (true)
            {
                try
                {
                    doPing();
                }
                catch (Exception exception)
                {
                    //reset sucessful ping counter
                    successfulPings = 0;
                    
                    Writer.printToConsole(DateTime.Now + ": The ping to " + this.ip + " failed: ", 0, true);
                    Writer.printToConsoleAndFile(DateTime.Now + ": ==============================", 0, true);
                    Writer.printToConsoleAndFile(DateTime.Now + ": " + exception.Message, 0, true);
                    Writer.printToConsoleAndFile(DateTime.Now + ": ==============================", 0, true);

                    //wait a second before retrying, not to flood console and log.
                    Thread.Sleep(1000);
                }
            }
        }

        private void doPing()
        {
            PingReply reply = this.Send(ip, 1000);

            if (reply.Status == IPStatus.Success)
            {
                failedPings = 0;
                successfulPings++;

                if (successfulPings >= 10)
                {
                    String text = DateTime.Now + ": Ping to " + ip.ToString() + " successful > 10 seconds. (" + successfulPings + "s)";
                    
                    Writer.printToConsoleAndFile(text, 4, true);
                }
                else
                {
                    String text = DateTime.Now + ": Ping to " + ip.ToString() + " successful. (" + successfulPings + "s)";

                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Writer.printToConsoleAndFile(text, 3, true);
                }

                //wait a second before sending next ping
                Thread.Sleep(1000);
            }
            else
            {
                failedPings++;
                if (failedPings >= 15)
                {
                    String text = DateTime.Now + ": Ping to " + ip.ToString() + " failed > 15 seconds. (" + failedPings + "s)";

                    Writer.printToConsoleAndFile(text, 1, true);
                }
                else
                {
                    String text = DateTime.Now + ": Ping to " + ip.ToString() + " failed. (" + failedPings + "s)";
                        
                    Writer.printToConsoleAndFile(text, 2, true);
                }
                
                //reset successful ping counter
                successfulPings = 0;            
            }

        }
    }
}