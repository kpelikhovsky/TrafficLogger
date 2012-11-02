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
      private StatCollectorCtrl ctrl;
      private System.Windows.Controls.ListView lw = new System.Windows.Controls.ListView();

      public delegate void UpdateStat( long received, long sent );
      public UpdateStat UpdDelegate;

      public MainWindow()
      {
         InitializeComponent();

         sCol = new StatCollector();
       
         InterfacesExpander.Content = lw;
         lw.SelectionChanged += lw_SelectionChanged;

         foreach (String conn in sCol.getActiveInterfaces())
         {
            lw.Items.Add(conn);
         }

         UpdDelegate = new UpdateStat(UpdateStatMethod);
      }

      void lw_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
      {
         InterfacesExpander.Header = lw.SelectedItem;
         InterfacesExpander.IsExpanded = false;
      }

      private void buttonStart_Click(object sender, RoutedEventArgs e)
      {
         String SelectedInterface = InterfacesExpander.Header.ToString();

         if ( "Please select an interface" != SelectedInterface)
         {
            ctrl = new StatCollectorCtrl(this, SelectedInterface, 1000);
            myThread = new Thread(new ThreadStart(ctrl.Run));
            myThread.Start();
         }
         else
         {
            MessageBox.Show( "Please select an interface" );
         }
      }

      private void buttonStop_Click(object sender, RoutedEventArgs e)
      {
         if (null != myThread)
         {
            ctrl.IsCtrlEnabled = false;
            myThread.Abort();
         }
      }

      public void UpdateStatMethod( long received, long sent )
      {
         tbReceived.Text = "Received Mb: " + (received / 1024).ToString();
         tbSent.Text = "Sent Mb: " + (sent / 1024).ToString();
      }
   }
}
