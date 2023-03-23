using System.ComponentModel;
using System.Windows;

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
