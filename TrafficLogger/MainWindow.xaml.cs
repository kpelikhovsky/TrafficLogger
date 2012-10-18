using System;
using System.Windows;
using System.Net.NetworkInformation;
using System.Threading;

namespace TrafficLogger
{
   /// <summary>
   /// Interaction logic for MainWindow.xaml
   /// </summary>
   
   public partial class MainWindow : Window
   {
      private Thread myThread = null;
      private StatCollector sCol;

      public delegate void UpdateStat( string str );
      public UpdateStat UpdDelegate;

      public MainWindow()
      {
         InitializeComponent();

         sCol = new StatCollector();
         foreach (System.Object conn in sCol.getInterfaces() )
         {
            SelConn.Items.Add(conn);
         }

         UpdDelegate = new UpdateStat(UpdateStatMethod);
      }

      private void buttonStart_Click(object sender, RoutedEventArgs e)
      {
         StatCollectorCtrl ctrl = new StatCollectorCtrl(this);
         myThread = new Thread(new ThreadStart(ctrl.Run));
         myThread.Start();
      }

      private void buttonStop_Click(object sender, RoutedEventArgs e)
      {
         if (null != myThread) myThread.Abort();
      }

      private void GetIf_Click(object sender, RoutedEventArgs e)
      {
         
      }

      public void UpdateStatMethod( string str )
      {
         textBlock1.Text = str;
      }
   }
}
