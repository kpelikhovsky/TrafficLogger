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
      private MainWindow pWin;
      private string nInterface;
      private int mInterval;
      private StatCollector sCol;
      private bool isCtrlEnabled;

      public StatCollectorCtrl(MainWindow win, string nIn, int mInt)
      {
         pWin = win;
         nInterface = nIn;
         mInterval = mInt;
         sCol = new StatCollector();
         isCtrlEnabled = true;
      }

      public bool IsCtrlEnabled
      {
         set
         {
            isCtrlEnabled = value;
         }
         
         get
         {
            return isCtrlEnabled;
         }
      }

      public void Run()
      {
         do
         {
            Thread.Sleep(mInterval);
            String str = "Packets received: " + sCol.getReceivedPackets(nInterface);
            pWin.Dispatcher.Invoke(pWin.UpdDelegate, DispatcherPriority.Normal, str);
         }
         while (isCtrlEnabled);
      }  
   }
   
   internal class StatCollector
   {
      public String[] getInterfaces()
      {
         IPGlobalProperties computerProperties = IPGlobalProperties.GetIPGlobalProperties();
         NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();

         String[] cInterfaces = new String[nics.Length];
         UInt16 it = 0;
         foreach (NetworkInterface adapter in nics)
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

      public long getReceivedPackets( String InterfaceName )
      {
         IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();
         IPGlobalStatistics ipstat = properties.GetIPv4GlobalStatistics();

         //NetworkInterface;
         /*switch (InterfaceName)
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
         }*/
        
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
