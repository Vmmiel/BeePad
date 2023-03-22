using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BeePad
{
    /// <summary>
    /// Lógica de interacción para ConfirmationWindow.xaml
    /// </summary>
    public partial class ConfirmationWindow : Window
    {
        public bool Yes { get; set; }
        public bool No { get; set; }
        public bool Cancel { get; set; }

        public ConfirmationWindow(string? title, string? yesContent, string? noContent, string? cancelContent, string? message)
        {
            InitializeComponent();

            Yes = false;
            No = false;
            Cancel = false;

            this.Title = title;

            YesButton.Content = yesContent;
            NoButton.Content = noContent;
            CancelButton.Content = cancelContent;

            MessageLabel.Content = message;
        }

        private void YesButton_Click(object sender, RoutedEventArgs e)
        {
            Yes = true; this.Close();
        }

        private void NoSaveButton_Click(object sender, RoutedEventArgs e)
        {
            No = true; this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Cancel = true; this.Close();
        }

        void ConfirmationWindow_OnClose(object sender, CancelEventArgs e)
        {
            Cancel = true;
        }
    }
}
