using Microsoft.WindowsAPICodePack.Net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;
using File = System.IO.File;

namespace NetworkCardCredentials
{
    internal class Functions
    {
        public static string GetIp()
        {
            string ip = "TakeIP";
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();

            foreach (NetworkInterface adapter in nics)
            {
                if (adapter.Name.Equals("Wi-Fi") || adapter.Name.Equals("Ethernet"))
                {
                    if (adapter.OperationalStatus == OperationalStatus.Up)
                    {
                        string mac_address = adapter.GetPhysicalAddress().ToString();

                        //Provides information about network interfaces
                        IPInterfaceProperties properties = adapter.GetIPProperties();

                        foreach (IPAddressInformation unicast in properties.UnicastAddresses)
                        {
                            if (unicast.Address.AddressFamily == AddressFamily.InterNetwork)
                            {
                                ip = unicast.Address.ToString();
                            }
                        }
                    }
                }
            }
            return ip;
        }
        public static string GetGateWay()
        {
            string gateway = "GW";
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface adapter in nics)
            {
                if (adapter.Name.Equals("Wi-Fi") || adapter.Name.Equals("Ethernet"))
                {
                    if (adapter.OperationalStatus == OperationalStatus.Up)
                    {
                        string mac_address = adapter.GetPhysicalAddress().ToString();
                        IPInterfaceProperties properties = adapter.GetIPProperties();

                        GatewayIPAddressInformationCollection addresses = properties.GatewayAddresses;
                        if (addresses.Count > 0)
                        {
                            foreach (GatewayIPAddressInformation address in addresses)
                            {
                                gateway = address.Address.ToString();
                            }
                        }
                    }
                }
            }
            return gateway;
        }
        public static string GetMacAddress()
        {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface adapter in nics)
            {
                if (adapter.Name.Equals("Wi-Fi") || adapter.Name.Equals("Ethernet"))
                {
                    if (adapter.OperationalStatus == OperationalStatus.Up)
                    {
                        string mac_address = adapter.GetPhysicalAddress().ToString();
                        return mac_address;
                    }
                }
            }
            return "Not Found";
        }

        public static string GetPing()
        {
            string ping = "";
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface adapter in nics)
            {
                if (adapter.Name.Equals("Wi-Fi") || adapter.Name.Equals("Ethernet"))
                {
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
                                ping = "Established";
                            }
                            if (PingInternetCounter > 0 && PingInternetCounter < 4)
                            {
                                ping = "Unstable";
                            }


                        }
                        if (reply.Status != IPStatus.Success)
                        {
                            ping = "Disconnect";
                        }
                    }
                }
            }
            return ping;
        }

        public static string GetSSID()
        {
            string SSID = "";
            string SSID1 = "";
            var networks = NetworkListManager.GetNetworks(NetworkConnectivityLevels.Connected);
            foreach (var network in networks)
            {
                if (network.IsConnected)
                {
                    SSID1 = "Connected";
                }
                else if (!network.IsConnected)
                {
                    SSID1 = "Disconnected";
                }
                SSID = network.Name;
            }
            return SSID;
        }
        public static string GetSSDIDStatus()
        {
            string SSID1 = "";
            var networks = NetworkListManager.GetNetworks(NetworkConnectivityLevels.Connected);
            foreach (var network in networks)
            {
                if (network.IsConnected)
                {
                    SSID1 = "Connected";
                }
                else if (!network.IsConnected)
                {
                    SSID1 = "Disconnected";
                }
            }
            return SSID1;
        }

        public static string VirtualBoxInstalled()
        {
            if (Directory.Exists(@"C:\Program Files\Oracle\VirtualBox"))
            {
                string[] files = Directory.GetFiles(@"C:\Program Files\Oracle\VirtualBox", "VBoxManage.exe", SearchOption.AllDirectories);

                if (files.Length > 0)
                {
                    return "Installed";
                }
                else
                {
                    return "Not installed";
                }
            }
            else
            {
                return "Not installed";
            }
            
        }
        public static string VirtualBoxVersion()
        {
            
                if (File.Exists("C:\\Program Files\\Oracle\\VirtualBox\\VBoxManage.exe"))
                {
                    var f = FileVersionInfo.GetVersionInfo("C:\\Program Files\\Oracle\\VirtualBox\\VBoxManage.exe").FileVersion;

                    return f == null ? "N/A" : f;
                }
                else
                {
                    return "N/A";
                }
            
                
        }


    }
}
