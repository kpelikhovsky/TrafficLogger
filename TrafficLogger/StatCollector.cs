using System;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Windows;

namespace TrafficLogger
{
   class StatCollector
   {
      /*
       * Get all active network interfaces
       * returns String[] array
      */
      public String[] getActiveInterfaces()
      {
         IPGlobalProperties computerProperties = IPGlobalProperties.GetIPGlobalProperties();
         NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
  
         int it = 0;
         foreach( NetworkInterface adapter in nics )
         {
            OperationalStatus sts = adapter.OperationalStatus;
            if (adapter.OperationalStatus == OperationalStatus.Up)
            {   
               ++it;
            }
         }

         String[] cInterfaces = new String[it];
         it = 0;
         foreach( NetworkInterface adapter in nics )
         {
            OperationalStatus sts = adapter.OperationalStatus;
            if (adapter.OperationalStatus == OperationalStatus.Up)
            {
               cInterfaces[it] = adapter.Name;
               ++it;
            }
         }

         return cInterfaces;
      }

      /*
       * Get received traffic
       * returns String[] array
      */
      public long getReceivedTraffic( NetworkInterface conn )
      {
         IPInterfaceProperties properties = conn.GetIPProperties();
         IPv4InterfaceStatistics ipstat = conn.GetIPv4Statistics();
        
         return ipstat.BytesReceived;
      }

      public long getSentPackets( NetworkInterface conn )
      {
          IPInterfaceProperties properties = conn.GetIPProperties();
          IPv4InterfaceStatistics ipstat = conn.GetIPv4Statistics();

         return ipstat.BytesSent;
      }
   }
}
