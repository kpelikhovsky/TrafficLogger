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

      public delegate void UpdateStat( String str);
      public UpdateStat UpdDelegate;

      public MainWindow()
      {
         InitializeComponent();

         sCol = new StatCollector();
         SelConn.Items.Insert(0, "Please select an interface");
         SelConn.SelectedIndex = 0;

         
         InterfacesExpander.Content = lw;
         lw.SelectionChanged += lw_SelectionChanged;

         foreach (String conn in sCol.getInterfaces() )
         {
            SelConn.Items.Add(conn);
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
         String SelectedInterface = SelConn.SelectionBoxItem.ToString();

         if ("Please select an interface" != SelectedInterface)
         {
            ctrl = new StatCollectorCtrl(this, SelectedInterface, 1000);
            myThread = new Thread(new ThreadStart(ctrl.Run));
            myThread.Start();
         }
         else
         {
            MessageBox.Show("Please select an interface");
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

      public void UpdateStatMethod( String str)
      {
         textBlock1.Text = str;
      }
   }
}
