using System;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Threading;
using System.Windows;

namespace TrafficLogger
{
   public class StatsChecker
   {
      private long _pReceived, _pSent;
      private StatCollector _sCol;

      private int invokeCount;
      private int maxCount;

      public StatsChecker( int cnt )
      {
         _pReceived = 0;
         _pSent = 0;
         _sCol = new StatCollector();

         invokeCount = 0;
         maxCount = cnt;
      }

      public void UpdateStats(Object stateInfo)
      {
         AutoResetEvent autoEvent = (AutoResetEvent)stateInfo;
         Console.WriteLine("{0} Checking status {1,2}.",
             DateTime.Now.ToString("h:mm:ss.fff"),
             (++invokeCount).ToString());

         if (invokeCount == maxCount)
         {
            // Reset the counter and signal Main.
            invokeCount = 0;
            autoEvent.Set();
         }
      }

      public void Run(object data)
      {
         //do
         //{
            MainWindow _w = (MainWindow)data;
            //Thread.Sleep(1000);
            _pReceived = _sCol.getReceivedPackets(NetworkInterfaceComponent.IPv4);
            _pSent = _sCol.getSentPackets(NetworkInterfaceComponent.IPv4);
            if (null != _w)
            {
                _w.onUpdate();

                //_w.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(() => { _w.textBlock1.Text = "Received Packets: " + _pReceived.ToString(); }));
                _w.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(() => { _w.textBlock2.Text = "Sent Packets: " + _pSent.ToString(); }));
            }
         //}
         //while (true);
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
