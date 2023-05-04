using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Socket_4I
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<string> Contatti { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            Contatti = new List<string>();  

            Contatti.Add("127.0.0.1");

            AggiornaLabel();
        }

        private void btnAggiungi_Click(object sender, RoutedEventArgs e)
        {
            AggiungiContatto ac = new AggiungiContatto();
            ac.ShowDialog();

            Contatti.Add(ac.Contatto);

            AggiornaLabel();
        }

        private void btnAvviaChat_Click(object sender, RoutedEventArgs e)
        {
            Window1 chat = new Window1(lstContatti.SelectedItem.ToString());
            chat.Show();
        }

        private void lstContatti_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnAvviaChat.IsEnabled = true;
            btnElimina.IsEnabled = true;
        }

        private void AggiornaLabel()
        {
            lstContatti.Items.Clear();  

            foreach (string contatto in Contatti)
            {
                lstContatti.Items.Add(contatto);
            }
        }

        private void btnElimina_Click(object sender, RoutedEventArgs e)
        {

            Contatti.Remove(lstContatti.SelectedItem.ToString());
            lstContatti.Items[lstContatti.SelectedIndex] = null;

            MessageBox.Show("Contatto eliminato con successo!");

            AggiornaLabel();
        }
    }
}
