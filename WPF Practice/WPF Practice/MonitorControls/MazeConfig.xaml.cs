﻿using System;
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
using System.Windows.Shapes;

namespace WPF_Practice.MonitorControls
{
    /// <summary>
    /// Interaction logic for MazeConfig.xaml
    /// </summary>
    public partial class MazeConfig : UserControl
    {
        public int MazeSize
        {
            get { return comboMazeSize.SelectedIndex; }
            set { comboMazeSize.SelectedIndex = value; }
        }

        public string MazePalletName
        {
            get { return textBoxPalletN.Text; }
            set { textBoxPalletN.Text = value; }
        }

        public String MazeView
        {
            get { return (comboAIView.SelectedValue as ComboBoxItem).Content.ToString(); }
            set { setMazeView(value, comboAIView); }
        }

        private void setMazeView(string view, ComboBox cb)
        {
            if (view.Equals("First Person"))
                cb.SelectedIndex = 0;
            else if (view.Equals("Top Down"))
                cb.SelectedIndex = 1;
        }

        public String SearchMethod
        {
            get { return (comboAIMethod.SelectedValue as ComboBoxItem).Content.ToString();}
            set { setSearchMethod(value, comboAIMethod); }
        }

        public MazeConfig()
        {
            InitializeComponent();
        }

        private void setSearchMethod(string method, ComboBox cb)
        {
            switch (method)
            {
                case "Depth First":
                    cb.SelectedIndex = 0;
                    break;
                case "Breadth First":
                    cb.SelectedIndex = 1;
                    break;
                case "Left Hand Rule":
                    cb.SelectedIndex = 2;
                    break;
            }

        }

        public void fillPage(GroupSetting gSettings, MonitorSetting mSettings)
        {
            this.MazeSize = gSettings.mazeSize;
            this.MazePalletName = gSettings.mazePalletName;
            this.MazeView = mSettings.aiView;
            this.SearchMethod = mSettings.aiMethod;
        }

        public void getPage(GroupSetting gSettings, int numberscreen)
        {
            gSettings.mazeSize = MazeSize;
            gSettings.mazePalletName = MazePalletName;
            gSettings.monitors[numberscreen].aiMethod = SearchMethod;
            gSettings.monitors[numberscreen].aiView = MazeView;
            gSettings.ssType = "Maze";
        }

        public void setGroupSettings(GroupSetting gSettings, int numberscreen)
        {
            MazeSize = gSettings.mazeSize;
            SearchMethod = gSettings.monitors[numberscreen].aiMethod;
            MazeView = gSettings.monitors[numberscreen].aiView;
            MazePalletName = gSettings.mazePalletName;
        } 

    }
}
