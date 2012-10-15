using System;
using System.Windows;
using System.Net.NetworkInformation;
using System.Timers;

namespace TrafficLogger
{
   /// <summary>
   /// Interaction logic for MainWindow.xaml
   /// </summary>
   
   public partial class MainWindow : Window
   {
      private Timer UpdateTimer;
      private StatCollector sCol;

      public MainWindow()
      {
         InitializeComponent();

         sCol = new StatCollector();
         foreach (System.Object conn in sCol.getInterfaces() )
         {
            SelConn.Items.Add(conn);
         }

         UpdateTimer = new Timer(1000);
         UpdateTimer.Elapsed += new ElapsedEventHandler(OnTimerEvent);
      }

      private void button1_Click(object sender, RoutedEventArgs e)
      {
         UpdateTimer.Enabled = true;
      }

      private void button2_Click(object sender, RoutedEventArgs e)
      {
         UpdateTimer.Enabled = false;
      }

      private void GetIf_Click(object sender, RoutedEventArgs e)
      {
         
      }

      private void OnTimerEvent(object source, ElapsedEventArgs e)
      {
         textBlock1.Text = sCol.getReceivedPackets(NetworkInterfaceComponent.IPv4).ToString();
      }
   }
}
