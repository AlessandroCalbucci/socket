﻿using System;
using System.Collections.Generic;
using System.Linq;
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

namespace Socket_4I
{
    /// <summary>
    /// Logica di interazione per AggiungiContatto.xaml
    /// </summary>
    public partial class AggiungiContatto : Window
    {
        public string Contatto { get; set; }

        public AggiungiContatto()
        {
            InitializeComponent();
        }

        private void btnAggiungi_Click(object sender, RoutedEventArgs e)
        {
            Contatto = txtIndirizzo.Text;

            this.Close();
        }
    }
}
