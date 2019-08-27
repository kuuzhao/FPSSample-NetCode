using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Net.Sockets;

public static class NetworkUtils
{
    public static List<string> GetLocalInterfaceAddresses()
    {
        // Useful to print 'best guess' for local ip, so...
        List<NetworkInterface> interfaces = new List<NetworkInterface>();
        List<string> addresses = new List<string>();
        foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces())
        {
            if (item.OperationalStatus != OperationalStatus.Up)
                continue;

            var type = item.NetworkInterfaceType;
            if (type != NetworkInterfaceType.Ethernet && type != NetworkInterfaceType.Wireless80211)
                continue;
            interfaces.Add(item);
        }

        // Sort interfaces so those with most gateways are first. Attempting to guess what is the 'main' ip address
        interfaces.Sort((a, b) => { return b.GetIPProperties().GatewayAddresses.Count.CompareTo(a.GetIPProperties().GatewayAddresses.Count); });

        foreach (NetworkInterface item in interfaces)
        {
            try
            {
                foreach (UnicastIPAddressInformation addr in item.GetIPProperties().UnicastAddresses)
                {
                    if (addr.Address.AddressFamily != AddressFamily.InterNetwork)
                        continue;
                    addresses.Add(addr.Address.ToString());
                }
            }
            catch (System.Exception e)
            {
                // NOTE : For some reason this can throw marshal exception in the interop 
                // to native network code on some computers (when running player but not in editor)?
                GameDebug.Log("Error " + e.Message + " while getting IP properties for " + item.Description);
            }
        }
        return addresses;
    }
}
