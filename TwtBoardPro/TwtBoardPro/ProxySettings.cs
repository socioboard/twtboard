using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RedTomahawk.TORActivator;
using System.Timers;
using BaseLib;

namespace ProxySettings
{
    public class ProxySettings
    {
        CEngine engine = new CEngine();

        public static int proxyTime = 120000;

        public static List<string> proxies = new List<string>();

        public static int i = 0;

        Timer ProxyTimer = new Timer(proxyTime);

        public static BaseLib.Events logEvents = new BaseLib.Events();

        public ProxySettings()
        {
            SetProxy(proxies[i]);
            ProxyTimer.Start();
            ProxyTimer.Elapsed += new ElapsedEventHandler(ProxyTimer_Elapsed);
        }

        public void SetProxy(string proxy)
        {
            engine.EnableProxy(proxy);
            Log("[ " + DateTime.Now + " ] => [ Current Proxy: " + proxy + " ]");
        }

        void ProxyTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (i < proxies.Count-1)
            {
                ProxyTimer.Stop();
                SetProxy(proxies[++i]); 
            }
            else
            {
                i = 0;
                ProxyTimer.Stop();
                SetProxy(proxies[i]);
            }
            ProxyTimer.Start();
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
