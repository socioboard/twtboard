/**
 * @author Sergey Kolchin <ksa242@gmail.com>
 */

using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;


namespace DeathByCaptcha
{
    /**
     * <summary>Death by Captcha socket API client.</summary>
     */
    public class SocketClient : Client
    {
        /**
         * <value>API host.</value>
         */
        public const string ServerHost = "api.deathbycaptcha.com";

        /**
         * <value>First API port.</value>
         */
        public const int ServerFirstPort = 8123;

        /**
         * <value>Last API port.</value>
         */
        public const int ServerLastPort = 8130;


        protected TcpClient _tcpClient = null;

        /**
         * <value>Flag whether the client is connected to the API.</value>
         */
        public bool Connected
        {
            get
            {
                return null != _tcpClient && _tcpClient.Connected;
            }
        }


        protected void Disconnect()
        {
            if (null != this._tcpClient) {
                this.Log("CLOSE");
                try {
                    this._tcpClient.GetStream().Close();
                    this._tcpClient.Close();
                } catch (System.Exception) {
                    //
                }
                this._tcpClient = null;
            }
        }

        protected bool Connect()
        {
            if (null != this._tcpClient && !this._tcpClient.Connected) {
                this._tcpClient = null;
            }
            if (null == this._tcpClient) {
                this.Log("CONN");
                this._tcpClient = new TcpClient();
                this._tcpClient.ReceiveTimeout = this._tcpClient.SendTimeout =
                    Client.DefaultTimeout * 1000;
                try {
                    this._tcpClient.Connect(SocketClient.ServerHost,
                                            new Random().Next(SocketClient.ServerFirstPort,
                                                              SocketClient.ServerLastPort + 1));
                } catch (System.Exception) {
                    this._tcpClient = null;
                    //throw new IOException("API connection failed");
                }
            }
            return this.Connected;
        }

        protected SocketClient Send(byte[] buf)
        {
            this.Log("SEND", Encoding.ASCII.GetString(buf, 0, buf.Length - 1));
            this._tcpClient.GetStream().Write(buf, 0, buf.Length);
            return this;
        }

        protected string Receive()
        {
            int n;
            byte[] buf = new byte[256];
            StringBuilder sb = new StringBuilder();
            NetworkStream st = this._tcpClient.GetStream();
            while (0 < (n = st.Read(buf, 0, buf.Length))) {
                sb.Append(Encoding.ASCII.GetString(buf, 0, n));
                if ('\n' == buf[n - 1]) {
                    break;
                }
            }
            this.Log("RECV", sb.ToString());
            return sb.ToString();
        }

        protected Hashtable Call(string cmd, Hashtable args)
        {
            args["cmd"] = cmd;
            args["version"] = Client.Version;

            byte[] payload = Encoding.ASCII.GetBytes(SimpleJson.Writer.Write(args) + "\n");
            Hashtable response = null;
            while (null == response) {
                lock (this._callLock) {
                    if (this.Connect()) {
                        string buf = null;
                        try {
                            buf = this.Send(payload).Receive();
                        } catch (System.Exception) {
                            this.Disconnect();
                        }
                        if (null != buf) {
                            try {
                                response = (Hashtable)SimpleJson.Reader.Read(buf);
                            } catch (FormatException) {
                                response = new Hashtable();
                            }
                        }
                    }
                }
            }

            try {
                if (null == response) {
                    //throw new IOException("API connection lost or timed out");
                }

                int status = 0xff;
                try {
                    status = Convert.ToInt32(response["status"]);
                } catch (System.Exception) {
                    //
                }
                if (0x00 < status && 0x10 > status) {
                    //throw new AccessDeniedException();
                } else if (0x10 <= status && 0x20 > status) {
                    //throw new InvalidCaptchaException();
                } else if (0xff == status) {
                    //throw new IOException("API server error occured");
                }
            } catch (System.Exception e) {
                lock (this._callLock) {
                    this.Disconnect();
                }
                //throw e;
            }

            return response;
        }

        protected Hashtable Call(string cmd)
        {
            return this.Call(cmd, new Hashtable());
        }


        /**
         * <see cref="M:DeathByCaptcha.Client(String, String)"/>
         */
        public SocketClient(string username, string password) : base(username, password)
        {}

        ~SocketClient()
        {
            this.Close();
        }


        /**
         * <see cref="M:DeathByCaptcha.Client.Close()"/>
         */
        public override void Close()
        {
            lock (this._callLock) {
                this.Disconnect();
            }
        }


        /**
         * <see cref="M:DeathByCaptcha.Client.GetUser()"/>
         */
        public override User GetUser()
        {
            return new User(this.Call("user", this.Credentials));
        }

        /**
         * <see cref="M:DeathByCaptcha.Client.GetCaptcha(int)"/>
         */
        public override Captcha GetCaptcha(int id)
        {
            Hashtable args = new Hashtable();
            args["captcha"] = id;
            return new Captcha(this.Call("captcha", args));
        }

        /**
         * <see cref="M:DeathByCaptcha.Client.Upload(byte[])"/>
         */
        public override Captcha Upload(byte[] img)
        {
            Hashtable args = this.Credentials;
            args["swid"] = Client.SoftwareVendorId;
            args["captcha"] = Convert.ToBase64String(img);
            Captcha c = new Captcha(this.Call("upload", args));
            return c.Uploaded ? c : null;
        }

        /**
         * <see cref="M:DeathByCaptcha.Client.Remove(int)"/>
         */
        public override bool Remove(int id)
        {
            Hashtable args = this.Credentials;
            args["captcha"] = id;
            return !(new Captcha(this.Call("remove", args))).Uploaded;
        }

        /**
         * <see cref="M:DeathByCaptcha.Client.Report(int)"/>
         */
        public override bool Report(int id)
        {
            Hashtable args = this.Credentials;
            args["captcha"] = id;
            return !(new Captcha(this.Call("report", args))).Correct;
        }
    }
}
