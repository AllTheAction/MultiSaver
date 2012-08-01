using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Diagnostics;
using System.Windows.Shapes;
using WPF_Practice.MonitorControls;

namespace WPF_Practice
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private GroupControl gcontrol = new GroupControl();
        private List<GroupSetting> groupsettings = new List<GroupSetting>();
        private List<List<string>> ownedmonitors = new List<List<string>>();
        private int currentActiveGroup;
        private int currentScreen = -1;

        bool firstAppear = true;

        public MainWindow()
        {

            List<string> tmpMonitors = new List<string>();
            InitializeComponent();
           // gcontrol.Visibility = System.Windows.Visibility.Hidden;
            mainControl.Children.Add(gcontrol);
            
        }

        private void Create_Button_Clicked(object sender, RoutedEventArgs e)
        {
            MonitorTab monitor = new MonitorTab();

            monitor.Width = 383;
            monitor.Height = 30;
            monitor.MinWidth = MonitorMenu.MinWidth;
            monitor.MaxWidth = MonitorMenu.MaxWidth;
            monitor.MouseDown += Monitor_clicked;
            monitor.order = this.createnewGroup();
            currentScreen = this.getTotalNumberofGroups() - 1;
            monitor.title = "New Group";
            MonitorMenu.Children.Add(monitor);
        }

        private void Monitor_clicked(object sender, EventArgs e)
        {
            MonitorTab tab = (MonitorTab)sender;
            MonitorTab prevtab = (MonitorTab)MonitorMenu.Children[currentScreen];
            prevtab.title = groupsettings[currentScreen].groupName;
            //resetting the color
            foreach(MonitorTab mt in MonitorMenu.Children)
                mt.Background = null;
            currentScreen = tab.order;
            tab.Background = Brushes.DarkTurquoise;
            
            gcontrol.Visibility = System.Windows.Visibility.Visible;
            //displayGroupControl(currentScreen);
            gcontrol.Load_Page(groupsettings[currentScreen]);
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {

            this.deleteGroup(currentScreen);

            for (int i = currentScreen +1;  i <= MonitorMenu.Children.Count-1 ; i++)
            {
                    MonitorTab tab = (MonitorTab)MonitorMenu.Children[i];
                    tab.order = i-1;
            }

            MonitorMenu.Children.RemoveAt(currentScreen);
            firstAppear = true;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {

            Window save = new Save(groupsettings);
            save.ShowDialog();
            //XMLHandler.save(this.getGroupSettings());
        }



        



        public int createnewGroup()
        {
            int groupid = groupsettings.Count + 1;
            List<string> tmpstring = new List<string>();
            groupsettings.Add(new GroupSetting());
            ownedmonitors.Add(new List<string>());
            groupsettings[groupsettings.Count - 1].groupName = "New Group";

            Debug.WriteLine(String.Format("GroupSettings: {0} Ownded MOnitors: {1}", groupsettings.Count, ownedmonitors.Count));

            return groupsettings.Count - 1;
        }
           

        public int getTotalNumberofGroups()
        {
            return groupsettings.Count;
        }

        public void displayGroupControl(int selectedScreen)
        {
            gcontrol.clear();
            Debug.WriteLine("Selected Screen: {0}", selectedScreen);
            gcontrol.groupSetting = groupsettings[selectedScreen];
            List<string> unassignedMonitors = new List<string>();
            int monitorcount = 1;
            foreach (System.Windows.Forms.Screen Screen in System.Windows.Forms.Screen.AllScreens)
            {
                string tmpMonitor ="Monitor " + monitorcount;
                if (!ownedmonitors[selectedScreen].Contains(Screen.DeviceName))
                {
                    unassignedMonitors.Add(tmpMonitor);
                }
                else
                {
                    gcontrol.OwnedMonitors.Add(tmpMonitor);
                }

            }
            gcontrol.AvailableMonitors = unassignedMonitors;
            currentActiveGroup = selectedScreen;
        }

        public void deleteGroup(int selectedScreen)
        {
            Debug.WriteLine("Selected Screen: {0}", selectedScreen);
            groupsettings.RemoveAt(selectedScreen);
            ownedmonitors.RemoveAt(selectedScreen);
            gcontrol.clear();
        }

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            List<GroupSetting> loaded = new List<GroupSetting>();
            loaded = XMLHandler.load("config.xml");
            foreach (GroupSetting gs in loaded)
            {
                MonitorTab monitor = new MonitorTab();
                groupsettings.Add(gs);
                monitor.Width = 383;
                monitor.Height = 30;
                monitor.MinWidth = MonitorMenu.MinWidth;
                monitor.MaxWidth = MonitorMenu.MaxWidth;
                monitor.MouseDown += Monitor_clicked;
                monitor.order = groupsettings.Count - 1;
                currentScreen = groupsettings.Count - 1;
                monitor.passtitleRef(ref gs.groupName);
                MonitorMenu.Children.Add(monitor);

                ownedmonitors.Add(new List<string>());
                foreach (MonitorSetting ms in gs.monitors)
                {
                    ownedmonitors[ownedmonitors.Count - 1].Add(ms.monitorId);
                }
            }
        }
    }
}
