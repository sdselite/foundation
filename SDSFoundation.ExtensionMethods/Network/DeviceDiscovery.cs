using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SDSFoundation.ExtensionMethods.Network
{
    public class DeviceDiscovery
    {
        List<string> ipAddresses = new List<string>();
        CountdownEvent countdown;
        int upCount = 0;
        object lockObj = new object();
        const bool resolveNames = true;

        public List<string> GetIPAddresses(int timeout = 100)
        {
            countdown = new CountdownEvent(1);
            Stopwatch sw = new Stopwatch();
            sw.Start();
            //string ipBase = "192.168.1.";
            string ipBase = GetLocalIpAddress(3);

            for (int i = 1; i < 255; i++)
            {
                string ip = ipBase + i.ToString();
                Ping p = new Ping();

                p.PingCompleted += new PingCompletedEventHandler(p_PingCompleted);
                countdown.AddCount();
                p.SendAsync(ip, timeout, ip);
            }
            countdown.Signal();
            countdown.Wait();
            sw.Stop();
            TimeSpan span = new TimeSpan(sw.ElapsedTicks);
            Debug.WriteLine("Took {0} milliseconds. {1} hosts active.", sw.ElapsedMilliseconds, upCount);

            return ipAddresses;
        }



        private void p_PingCompleted(object sender, PingCompletedEventArgs e)
        {
            string ip = (string)e.UserState;
            if (e.Reply != null && e.Reply.Status == IPStatus.Success)
            {
                ipAddresses.Add(ip);

                Debug.WriteLine("{0}  is up: ({1} ms)", ip, e.Reply.RoundtripTime);

                lock (lockObj)
                {
                    upCount++;
                }
            }
            else if (e.Reply == null)
            {
                Debug.WriteLine("Pinging {0} failed. (Null Reply object?)", ip);
            }
            countdown.Signal();
        }

        /// <summary>
        /// returns a partial ip address, or whole
        /// </summary>
        /// <param name="length">Valid Values: 1-4</param>
        /// <returns></returns>
        private string GetLocalIpAddress(int length)
        {
            string result = string.Empty;
            String strHostName = string.Empty;
            // Getting Ip address of local machine...
            // First get the host name of local machine.
            strHostName = Dns.GetHostName();
            Console.WriteLine("Local Machine's Host Name: " + strHostName);
            // Then using host name, get the IP address list..
            IPHostEntry ipEntry = Dns.GetHostEntry(strHostName);
            IPAddress[] addr = ipEntry.AddressList;

            foreach (var currentAddress in addr)
            {
                if (currentAddress.AddressFamily == AddressFamily.InterNetwork)
                {
                    var addressSplit = currentAddress.ToString().Split('.')?.ToList();

                    if (addressSplit != null && addressSplit.Count > 0)
                    {
                        foreach (var addressPart in addressSplit)
                        {
                            if (addressSplit.IndexOf(addressPart) <= length - 1)
                            {
                                result = string.Format("{0}{1}.", result, addressPart);
                            }

                        }
                    }


                }
            }

            return result;
        }
    }
}
