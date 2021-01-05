using System;
using System.Net;
using System.Text.RegularExpressions;

namespace UnifiedNetwork
{
    public class ConnectionUtils
    {
        /// <summary>
        /// Get IPEndPoint from host or IP
        /// </summary>
        /// <param name="hostOrIP"></param>
        /// <param name="port"></param>
        /// <returns>IPEndPoint of host</returns>
        public static IPEndPoint GetIPEndPoint(string hostOrIP, int port, bool preferIPv4 = true)
        {
            IPEndPoint ipEndPoint = null;

            try
            {
                if (string.IsNullOrEmpty(hostOrIP) == false)
                {
                    IPAddress address = null;
                    HostType hostType = GetHostType(hostOrIP);

                    if (hostType == HostType.DOMAIN)
                    {
                        IPAddress[] addressList = GetAddressList(hostOrIP);
                        address = GetIPAddressFromList(addressList, preferIPv4);
                    }
                    else //Is IP address
                    {
                        address = IPAddress.Parse(hostOrIP);
                    }

                    // Instantiates the endpoint
                    ipEndPoint = new IPEndPoint(address, port);
                }
                else
                {
                    UnifiedLogger.Warn(Tag.CONNECTION_UTILS, "Error get IPEndPoint: host is null or empty");
                }
            }
            catch (Exception e)
            {
                UnifiedLogger.Error(Tag.CONNECTION_UTILS, string.Format("Error get IPEndPoint of {0} : {1}", hostOrIP, e));
            }

            return ipEndPoint;
        }

        public static HostType GetHostType(string hostOrIP)
        {
            HostType hostType = HostType.IPv4;

            Regex regex = new Regex(Constants.IPv4ValidatorString);
            if (regex.IsMatch(hostOrIP))
            {
                hostType = HostType.IPv4;
            }
            else
            {
                if (hostOrIP.Contains("."))
                {
                    hostType = HostType.DOMAIN;
                }
                else
                {
                    hostType = HostType.IPv6;
                }
            }

            return hostType;
        }

        public static IPAddress GetIPAddressFromList(IPAddress[] addressList, bool preferIPv4 = true)
        {
            IPAddress result = null;
            if (addressList != null && addressList.Length > 0)
            {
                result = addressList[0];

                if (addressList.Length > 1)
                {
                    if (preferIPv4)
                    {
                        for (int i = 0; i < addressList.Length; i++)
                        {
                            if (addressList[i].AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                            {
                                result = addressList[i];
                                break;
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < addressList.Length; i++)
                        {
                            if (addressList[i].AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
                            {
                                result = addressList[i];
                                break;
                            }
                        }
                    }
                }
            }

            return result;
        }

        public static IPAddress[] GetAddressList(string domain)
        {
            IPAddress[] addressList = null;

            try
            {
                IPHostEntry hostEntry = Dns.GetHostEntry(domain);
                addressList = hostEntry.AddressList;

                UnifiedLogger.Info(Tag.CONNECTION_UTILS, string.Format("Number Address of {0} is {1}", domain, addressList.Length));
                for (int i = 0; i < addressList.Length; i++)
                {
                    UnifiedLogger.Info(Tag.CONNECTION_UTILS, string.Format("Address {0} of {1} is {2}", i, domain, addressList[i]));
                }
            }
            catch (Exception e)
            {
                UnifiedLogger.Exception(Tag.CONNECTION_UTILS, e);
            }

            return addressList;
        }
    }
}