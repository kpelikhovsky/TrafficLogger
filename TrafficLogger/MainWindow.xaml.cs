using System;
using System.Windows;
using System.Threading;
using System.Net.NetworkInformation;

namespace TrafficLogger
{
   /// <summary>
   /// Interaction logic for MainWindow.xaml
   /// </summary>

   public delegate int onInfoUpdate(int value1 );
   
   public partial class MainWindow : Window
   {
      private Thread thr;
      AutoResetEvent autoEvent;
      StatsChecker stchk;
      TimerCallback tcb;
      private Timer t;

      public MainWindow()
      {
         InitializeComponent();
         
         StatCollector a = new StatCollector();
         foreach ( System.Object conn in a.getInterfaces())
         {
            SelConn.Items.Add(conn);
         }
         autoEvent = new AutoResetEvent(false);
         stchk = new StatsChecker(10);
         tcb = stchk.UpdateStats;
         t = new Timer( tcb, autoEvent, 1000, 250 );
      }

      public void onUpdate()
      {
         textBlock1.Text = "Fuck Yeah!";
      }

      private void button1_Click(object sender, RoutedEventArgs e)
      {
         autoEvent.WaitOne(5000, false);
         t = new Timer(tcb, autoEvent, 1000, 250);
      }

      private void button2_Click(object sender, RoutedEventArgs e)
      {
         t.Dispose();
      }

      private void GetIf_Click(object sender, RoutedEventArgs e)
      {
         
      }
      
   }
}
