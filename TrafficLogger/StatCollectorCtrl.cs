using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading;
using System.Windows.Threading;
using System.Net.NetworkInformation;

namespace TrafficLogger
{
   public class StatCollectorCtrl
   {
      private MainWindow pWin;               // reference to UI window to invoke update
      private string mSelectedInterface;     // interface selected by user
      private int mUpadateInterval;          // how often need to get statistics
      private StatCollector sCol;
      private bool isCtrlEnabled;
      private NetworkInterface conn;

      private long mReceivedBytes;
      private long mSentBytes;
  
      public StatCollectorCtrl(MainWindow win, string nIn, int mInt)
      {
         pWin = win;
         mSelectedInterface = nIn;
         mUpadateInterval = mInt;
         sCol = new StatCollector();
         isCtrlEnabled = true;
         if (true == setActiveInterface(mSelectedInterface))
         { 
            
         }
      }

      // Accessors
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

      public bool setActiveInterface(String SelectedInterface)
      {
         IPGlobalProperties computerProperties = IPGlobalProperties.GetIPGlobalProperties();
         NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();

         bool isSet = false;

         foreach (NetworkInterface adapter in nics)
         {
            if (adapter.Name == SelectedInterface)
            {
               conn = adapter;
               isSet = true;
            }
         }
         return isSet;
      }

      public void Run()
      {
         do
         {
            Thread.Sleep(mUpadateInterval);
            mReceivedBytes = sCol.getReceivedTraffic(conn);
            mSentBytes = sCol.getSentPackets(conn);
            pWin.Dispatcher.Invoke(pWin.UpdDelegate, DispatcherPriority.Normal, mReceivedBytes, mSentBytes);
         }
         while (isCtrlEnabled);
      }
   }
}
