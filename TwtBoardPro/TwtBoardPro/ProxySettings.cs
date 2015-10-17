using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RedTomahawk.TORActivator;
using System.Timers;
using BaseLib;

namespace IPSettings
{
    public class IPSettings
    {
        CEngine engine = new CEngine();

        public static int IPTime = 120000;

        public static List<string> proxies = new List<string>();

        public static int i = 0;

        Timer IPTimer = new Timer(IPTime);

        public static BaseLib.Events logEvents = new BaseLib.Events();

        public IPSettings()
        {
            SetIP(proxies[i]);
            IPTimer.Start();
            IPTimer.Elapsed += new ElapsedEventHandler(IPTimer_Elapsed);
        }

        public void SetIP(string IP)
        {
            engine.EnableProxy(IP);
            Log("[ " + DateTime.Now + " ] => [ Current IP: " + IP + " ]");
        }

        void IPTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (i < proxies.Count-1)
            {
                IPTimer.Stop();
                SetIP(proxies[++i]); 
            }
            else
            {
                i = 0;
                IPTimer.Stop();
                SetIP(proxies[i]);
            }
            IPTimer.Start();
        }

        /// <summary>
        /// Adds to logger list box in Mainfrm...
        /// </summary>
        /// <param name="log">String to log</param>
        public void Log(string log)
        {
            EventsArgs eArgs = new EventsArgs(log);
            logEvents.LogText(eArgs);
        }
    }
}
