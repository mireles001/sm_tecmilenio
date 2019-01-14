using System.Net.NetworkInformation;

public class IP
{
  private string _ip;

  public IP()
  {
    foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
    {
      if (ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 || ni.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
      {
        foreach (UnicastIPAddressInformation ip in ni.GetIPProperties().UnicastAddresses)
        {
          if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
          {
            _ip = ip.Address.ToString();
            break;
          }
        }
      }

      if (_ip.Length > 0)
        break;
    }
  }

  public string GetIp()
  {
    return _ip;
  }
}
