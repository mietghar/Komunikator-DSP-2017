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
        }

        private void btClientRun_Click(object sender, RoutedEventArgs e)
        {
            Thread klient = new Thread(delegate ()
            {
                MainWindow Klient = new MainWindow();
                Klient.Show();
                System.Windows.Threading.Dispatcher.Run();
            });
            klient.SetApartmentState(ApartmentState.STA);
            klient.Start();
        }

        private void btnServerRun_Click(object sender, RoutedEventArgs e)
        {
            Thread serwer = new Thread(delegate ()
            {
                TCPServer Serwer = new TCPServer();
                Serwer.Show();
                System.Windows.Threading.Dispatcher.Run();
            });
            serwer.SetApartmentState(ApartmentState.STA);
            serwer.Start();
        }
    }
}
