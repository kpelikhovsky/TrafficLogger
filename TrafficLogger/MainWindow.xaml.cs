using System;
using System.Windows;
using System.Windows.Controls;
using System.Net.NetworkInformation;
using System.Threading;
using Hardcodet.Wpf.TaskbarNotification;

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
      private ListView lw = new ListView();

      // Create tray icon
      private TaskbarIcon tbi; 

      // Delegate method for data update
      public delegate void UpdateStat( long received, long sent );
      public UpdateStat UpdDelegate;

      public MainWindow()
      {
         InitializeComponent();

         sCol = new StatCollector();
         
         // Init listview for expander
         InterfacesExpander.Content = lw;
         lw.SelectionChanged += lw_SelectionChanged;

         foreach (String conn in sCol.getActiveInterfaces())
         {
            lw.Items.Add(conn);
         }

         // Set delagate method 
         UpdDelegate = new UpdateStat(UpdateStatMethod);

         // Init tray icon
         tbi = new TaskbarIcon();
         tbi.Icon = Properties.Resources.DefaultIcon;
         tbi.ToolTipText = "hello world";
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
