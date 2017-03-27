using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using komunikator.Models;
using System.Net;
using komunikator.Models.Databases;
using System.Data.SQLite;

namespace komunikator
{
    /// <summary>
    /// Interaction logic for StartUpWindow.xaml
    /// </summary>
    public partial class StartUpWindow : Window
    {
        public StartUpWindow()
        {
            InitializeComponent();
            Color red = (Color)ColorConverter.ConvertFromString("#FFFF3A3A");
            btnServerRun.Background = new SolidColorBrush(red);
        }
        private int serverCounter = 0;
        IServer server;

        private void btClientRun_Click(object sender, RoutedEventArgs e)
        {
            Thread klient = new Thread(delegate ()
            {
                MainWindow Klient = new MainWindow();
                Klient.Show();
                System.Windows.Threading.Dispatcher.Run();
            });
            klient.SetApartmentState(ApartmentState.STA);
            //przeniesienie wątku do tła, dzięki temu przy zamykaniu aplikacji zostanie on automatycznie zamknięty
            klient.IsBackground = true;
            klient.Start();
        }

        private void CheckServerButton(IServer server)
        {
            Color green = (Color)ColorConverter.ConvertFromString("#FF33D123");
            Color red = (Color)ColorConverter.ConvertFromString("#FFFF3A3A");

            if (this.server.IsStarted)
            {
                btnServerRun.Background = new SolidColorBrush(green);
            }
            else
            {
                btnServerRun.Background = new SolidColorBrush(red);
            }
        }

        private void btnServerRun_Click(object sender, RoutedEventArgs e)
        {
            serverCounter++;
            try {
                if (serverCounter % 2 != 0)
                {
                    server = new Server((int)Properties.Settings.Default["ServerPortSetting"], IPAddress.Parse((string)Properties.Settings.Default["ServerIPSetting"]));
                    server.Start();
                    server.ProcessConnection();
                }
                else server.Stop();
                CheckServerButton(server);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Coś poszło nie tak.");
            }
            try
            {
                ServerDataBase database = new ServerDataBase();
                database.DBPath = Properties.Settings.Default["DataBasePathSetting"].ToString();
                database.Name = Properties.Settings.Default["DataBaseNameSetting"].ToString();
                if (!database.Exists)
                {
                    database.GenerateServerDataBaseForFirstTime();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ExitFromMenuFile_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AboutMenu_Click(object sender, RoutedEventArgs e)
        {
            About about = new About();
            about.ShowDialog();
        }

        private void SettingsMenu_Click(object sender, RoutedEventArgs e)
        {
            MainSettingsMenu about = new MainSettingsMenu();
            about.ShowDialog();
        }
    }
}
