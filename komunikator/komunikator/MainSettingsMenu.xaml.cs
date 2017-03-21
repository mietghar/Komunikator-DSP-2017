﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace komunikator
{
    /// <summary>
    /// Interaction logic for MainSettingsMenu.xaml
    /// </summary>
    public partial class MainSettingsMenu : Window
    {
        public MainSettingsMenu()
        {
            InitializeComponent();
            LoadDefaultSettings();
        }

        private void LoadDefaultSettings()
        {
            //rewrite of default settings to window fields
            ServerIPTextBox.Text = Properties.Settings.Default["ServerIPSetting"].ToString();
            HostIPTextBox.Text = Properties.Settings.Default["HostIPSetting"].ToString();
            ServerPortTextBox.Text = Properties.Settings.Default["ServerPortSetting"].ToString();
            ClientPortTextBox.Text = Properties.Settings.Default["ClientPortSetting"].ToString();
        }

        private void SaveSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //ServerIPSetting
                IPAddress address = null;
                int port = 0;
                if (IPAddress.TryParse(ServerIPTextBox.Text, out address))
                {
                    Properties.Settings.Default["ServerIPSetting"] = ServerIPTextBox.Text;
                    Properties.Settings.Default.Save();
                }
                else
                {
                    throw new Exception("Can't save. Server IP address is wrong.");
                }

                //ServerPortSetting
                if (Int32.TryParse(ServerPortTextBox.Text, out port)==true && Int32.Parse(ServerPortTextBox.Text)>=0 && Int32.Parse(ServerPortTextBox.Text)<= 65535)
                {
                    Properties.Settings.Default["ServerPortSetting"] = Int32.Parse(ServerPortTextBox.Text);
                    Properties.Settings.Default.Save();
                }
                else
                {
                    throw new Exception("Can't save. Server port is wrong.");
                }
                //ClientPortSetting
                if (Int32.TryParse(ClientPortTextBox.Text, out port) == true && Int32.Parse(ClientPortTextBox.Text) >= 0 && Int32.Parse(ClientPortTextBox.Text) <= 65535)
                {
                    Properties.Settings.Default["ClientPortSetting"] = Int32.Parse(ServerPortTextBox.Text);
                    Properties.Settings.Default.Save();
                }
                else
                {
                    throw new Exception("Can't save. Client port is wrong.");
                }

                //HostIPSetting
                if (IPAddress.TryParse(HostIPTextBox.Text, out address))
                {
                    Properties.Settings.Default["HostIPSetting"] = HostIPTextBox.Text;
                    Properties.Settings.Default.Save();
                }
                else
                {
                    throw new Exception("Can't save. Host IP address is wrong.");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void LoadFactorySetting(string settingName, object value)
        {
            string typeName = null;
            if (Properties.Settings.Default[settingName] != null)
            {
                typeName = Properties.Settings.Default[settingName].GetType().Name;
            }
            else return;

            //integers group
            if (typeName == "Int32")
            {
                if (value.GetType().Name == "Int32")
                {
                    Properties.Settings.Default[settingName] = value;
                    Properties.Settings.Default.Save();
                }
                else throw new Exception("Wrong setting type. Should be Int32.");
            }
            //strings group
            if (typeName == "String" && value.GetType().Name == "String")
            {
                if(value.GetType().Name=="String")
                {
                    Properties.Settings.Default[settingName] = value;
                    Properties.Settings.Default.Save();
                }
                else throw new Exception("Wrong setting type. Should be String.");
                return;
            }
        }

        private void LoadFactoryDefaultSettings_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult settingsFactoryLoadResult = MessageBox.Show("That will clear your settings.","Proceed?",MessageBoxButton.YesNo);
            if (settingsFactoryLoadResult == MessageBoxResult.Yes)
            {
                try
                {
                    LoadFactorySetting("ServerPortSetting", 13000);
                    LoadFactorySetting("ClientPortSetting", 13000);
                    LoadFactorySetting("ServerIPSetting", "127.0.0.1");
                    LoadFactorySetting("HostIPSetting", "127.0.0.1");
                    //fullfill all fields again
                    LoadDefaultSettings();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
            else if (settingsFactoryLoadResult == MessageBoxResult.No)
            {
                //NOP
            }
            
            /*ServerIPTextBox.Text = Properties.Settings.Default["ServerIPSetting"].ToString();
            ServerPortTextBox.Text = Properties.Settings.Default["ServerPortSetting"].ToString();
            ClientPortTextBox.Text = Properties.Settings.Default["ClientPortSetting"].ToString();*/
        }
    }
}
