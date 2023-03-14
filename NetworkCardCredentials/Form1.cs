using System.Net.Sockets;
using System.Net;
using System.Net.NetworkInformation;
using System.Drawing.Text;
using System.Security.Cryptography.X509Certificates;
using System.Data.Common;
using Microsoft.VisualBasic.Devices;
using Microsoft.WindowsAPICodePack.Net;

namespace NetworkCardCredentials
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            string username = Environment.UserName;
            lbl_Username.Text = "Username:" + username;
            string HostName = Dns.GetHostName();
            lbl_Hostname.Text = "IPHost: " + HostName;
            lbl_IPHost.Text = Functions.GetIp();
            lbl_IPGateaway.Text = Functions.GetGateWay();
            lbl_MacAdress.Text = Functions.GetMacAddress(); 
            lbl_InternetCon.Text = Functions.GetPing();
            lbl_SSID.Text = Functions.GetSSID();
            lbl_SSIDStatus.Text = Functions.GetSSDIDStatus();
            lbl_VirtualBoxIns.Text = Functions.VirtualBoxInstalled();
            lbl_VirtualBoxVer.Text = Functions.VirtualBoxVersion();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            /*This class encapsulates data for network interfaces,
            also known as adapters, on the local computer.*/

            //GetAllNetInterfaceds: Returns objects that describe the network interfaces on the local computer.
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();

            foreach (NetworkInterface adapter in nics)
            {
                if (adapter.Name.Equals("Wi_fi") || adapter.Name.Equals("Ethernet"))
                {
                    if (adapter.OperationalStatus == OperationalStatus.Up)
                    {
                        string mac_address = adapter.GetPhysicalAddress().ToString();
                        lbl_MacAdress.Text = mac_address;

                        //Provides information about network interfaces
                        IPInterfaceProperties properties = adapter.GetIPProperties();

                        foreach (IPAddressInformation unicast in properties.UnicastAddresses)
                        {
                            if (unicast.Address.AddressFamily == AddressFamily.InterNetwork)
                            {
                                //lbl_IPHost.Text = unicast.Address.ToString();
                            }
                        }
                        GatewayIPAddressInformationCollection addresses = properties.GatewayAddresses;
                        if (addresses.Count > 0)
                        {
                            foreach (GatewayIPAddressInformation address in addresses)
                            {
                                lbl_IPGateaway.Text = address.Address.ToString();
                            }
                        }
                        Ping myPing = new Ping();
                        PingReply reply = myPing.Send("1.1.1.1", 1000);
                        int PingInternetCounter = 0;
                        for (int numpings = 0; numpings < 4; numpings++)
                        {
                            if (reply.Status == IPStatus.Success)
                            {
                                PingInternetCounter++;
                                if (PingInternetCounter.Equals(4))
                                {
                                    lbl_InternetCon.Text = "Established";
                                }
                                if (PingInternetCounter > 0 && PingInternetCounter < 4)
                                {
                                    lbl_InternetCon.Text = "Unstable";
                                }


                            }
                            if (reply.Status != IPStatus.Success)
                            {
                                lbl_InternetCon.Text = "Disconnect";
                            }
                        }
                    }
                }
            }

            var networks = NetworkListManager.GetNetworks(NetworkConnectivityLevels.Connected);
            foreach (var network in networks)
            {
                if (network.IsConnected)
                {
                    lbl_SSIDStatus.Text = "Connected";
                }
                else if (!network.IsConnected)
                {
                    lbl_SSIDStatus.Text = "Disconnected";
                }
                lbl_SSID.Text = network.Name;
            }
        }
    }
}