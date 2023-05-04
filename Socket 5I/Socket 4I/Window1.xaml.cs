using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
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
using System.Windows.Threading;
using System.Threading;

namespace Socket_4I
{
    /// <summary>
    /// Logica di interazione per Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        private string Contatto { get; set; }

        private int _portaAscolto;
        private int  PortaAscolto {
            get => _portaAscolto;
            set 
            {
                if (value > 1024 && value < 65535 || value == 0)
                    _portaAscolto = value;
                else
                    throw new Exception("Porta Ascolto non valida");
            }
        }

        private int _portaInvio;
        private int PortaInvio {
            get => _portaInvio;
            set
            {
                if (value > 1024 && value < 65535 || value == 0)
                    _portaInvio = value;
                else
                    throw new Exception("Porta Invio non valida");
            }
        } 

        Socket socket = null;
        DispatcherTimer dTimer = null;


        public Window1(string contatto)
        {
            InitializeComponent();

            Contatto = contatto;
            lblContatto.Content = Contatto;
        }

        private void CreazioneSocket()
        {
            /*classe socket:
             * AddressFamily.InterNetwork = Specifica lo schema di indirizzamento utilizzabile dalla classe socket, in questo caso IPv4
             * SocketType.Dgram = Specifica il tipo di socket, in questo caso supporta messaggi senza connessione, non affidabili di lunghezza massima fissa. E' in grado di
             * comunicare con più peer.
             * ProtocolType.Udp = Indica il tipo di protocollo di connessione, questo caso UDP
             */
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            //IPAddress.Any indica che il server può ricevere attività su tutte le interfacce di rete
            IPAddress local_address = IPAddress.Any;

            //Rappresenta l'endpoint con relativa porta in cui questo host ascolta i messaggi
            IPEndPoint local_endpoint = new IPEndPoint(local_address, PortaAscolto);

            //Associa l'endpoint alla socket
            socket.Bind(local_endpoint);

            //indica se la socket è in modalità di blocco
            socket.Blocking = false;

            //abilita la comunicazione broadcast
            socket.EnableBroadcast = true;



            #region Azione da svolgere nel task
            Action azione = () =>
            {
                int nBytes = 0;

                while (true)
                {
                    if ((nBytes = socket.Available) > 0)
                    {
                        //ricezione dei caratteri in attesa
                        byte[] buffer = new byte[nBytes];

                        //creazione endpoint su porta dinamica
                        EndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);

                        //metodo per ricevere messaggi dall'endpoint
                        nBytes = socket.ReceiveFrom(buffer, ref remoteEndPoint);

                        //indirizzo ip dell'endpoint
                        string from = ((IPEndPoint)remoteEndPoint).Address.ToString();

                        //conversione messaggio da array di byte a stringa utf8
                        string messaggio = Encoding.UTF8.GetString(buffer, 0, nBytes);

                        //permette di utilizzare il metodo appartenente ad un altro thread
                        Dispatcher.Invoke(() =>
                        {
                            lstChat.Items.Add(from + ": " + messaggio);
                        });
                    }

                    Thread.Sleep(150);
                }
            };
            #endregion

            //assegno l'azione al task
            Task riceviMessaggi = new Task(azione);

            //avvio il task
            riceviMessaggi.Start();
        }

        private void btnInvia_Click(object sender, RoutedEventArgs e)
        {
            //converto l'indirizzo ip da stringa a IPAddress
            IPAddress remote_address = IPAddress.Parse(Contatto);

            //creo un endpoint con l'indirizzo ip e la porta sui cui inviare i messaggi
            IPEndPoint remote_endpoint = new IPEndPoint(remote_address, PortaInvio);

            //trasformo da stringa ad array di byte
            byte[] messaggio = Encoding.UTF8.GetBytes(txtMessaggio.Text);

            lstChat.Items.Add("Tu: " + txtMessaggio.Text);

            //invio messaggio all'endpoint
            socket.SendTo(messaggio, remote_endpoint);

            //pulisco la textbox
            txtMessaggio.Text = null;
        }

        //metodo utilizzato per l'assegnazione delle porte, creo la socket solo una volta avute tutte le porte
        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PortaAscolto = int.Parse(txtPorta.Text);

                PortaInvio = int.Parse(txtPortaInvio.Text);

                CreazioneSocket();

                btnInvia.IsEnabled = true;
            }
            catch(Exception ex) 
            {
                MessageBox.Show("Errore: " + ex.Message);
            }
        }
    }
}
