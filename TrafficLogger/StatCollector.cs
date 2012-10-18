using System;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Threading;
using System.Windows.Threading;
using System.Windows;

namespace TrafficLogger
{
   public class StatCollectorCtrl
   {
      MainWindow pWin;
      public StatCollectorCtrl(MainWindow win)
      {
         pWin = win;
      }

      public void Run()
      {
         for (int i = 0; i < 10; ++i)
         {
            Thread.Sleep(1000);
            String str = "Delegate was called " + i.ToString();
            pWin.Dispatcher.Invoke(pWin.UpdDelegate, DispatcherPriority.Normal, str);
         }
      }  
   }
   
   
   
   internal class StatCollector
   {
      public System.Object[] getInterfaces()
      {
         IPGlobalProperties computerProperties = IPGlobalProperties.GetIPGlobalProperties();
         NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();

         System.Object[] _cInterfaces = new System.Object[nics.Length];
         UInt16 it = 0;
         foreach (NetworkInterface adapter in nics)
         {
            OperationalStatus sts = adapter.OperationalStatus;
            if (adapter.OperationalStatus == OperationalStatus.Up)
            {
               _cInterfaces[it] = adapter.Name;
               ++it;
            }
         }
         return _cInterfaces;
      }

      public long getReceivedPackets(NetworkInterfaceComponent version)
      {
         IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();
         IPGlobalStatistics ipstat = null;
         switch (version)
         {
            case NetworkInterfaceComponent.IPv4:
               ipstat = properties.GetIPv4GlobalStatistics();
               break;
            case NetworkInterfaceComponent.IPv6:
               ipstat = properties.GetIPv6GlobalStatistics();
               break;
            default:
               throw new ArgumentException("version");
               //    break;
         }
        
         return ipstat.ReceivedPackets;
      }

      public long getSentPackets(NetworkInterfaceComponent version)
      {
         //version = NetworkInterfaceComponent.IPv4;
         IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();
         IPGlobalStatistics ipstat = null;
         switch (version)
         {
            case NetworkInterfaceComponent.IPv4:
               ipstat = properties.GetIPv4GlobalStatistics();
               break;
            case NetworkInterfaceComponent.IPv6:
               ipstat = properties.GetIPv6GlobalStatistics();
               break;
            default:
               throw new ArgumentException("version");
            //    break;
         }

         return ipstat.OutputPacketRequests;
      }
   }
}
