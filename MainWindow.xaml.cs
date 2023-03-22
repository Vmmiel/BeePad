using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BeePad
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static class SupportedFileFormats
        {
            public const string TXT = "(*.txt) | *.txt";
        };

        public static class PageFile
        {
            public static string? DocumentPath { get; set; }
            public static string? CurrentText { get; set; }
            public static float CurrentZoom { get; set; }
            public static int DefaultFontSize { get; set; }
        };

        public MainWindow()
        {
            InitializeComponent();

            PageFile.CurrentText = string.Empty;
            PageFile.CurrentZoom = 1;
            PageFile.DefaultFontSize = 15; TextBoxPage.FontSize = PageFile.DefaultFontSize;

            NewFile();

            this.Loaded += new RoutedEventHandler(MainWindow_OnLoad);
        }

        public void SetWindowTitle()
        {
            this.Title = "Editing: " + "\"" + PageFile.DocumentPath + "\"" + " - " + Application.Current.Properties["AppName"]?.ToString();
        }

        public void NewCurrentText()
        {
            PageFile.CurrentText = TextBoxPage.Text;
        }

        public bool CheckUnsavedChanges()
        {
            if (PageFile.CurrentText != TextBoxPage.Text)
            {
                ConfirmationWindow window = new ConfirmationWindow
                (
                    Application.Current.Properties["AppName"].ToString(), 
                    "Save", 
                    "Don't save", 
                    "Cancel", 
                    "Do you want to save your changes?"
                );
                window.ShowDialog();

                if (window.Yes)    return SaveFile();
                if (window.No)     return true;
                if (window.Cancel) return false;
            }
            return true;
        }

        public void NewFile()
        {
            if (CheckUnsavedChanges())
            {
                PageFile.DocumentPath = "Untitled";
                SetWindowTitle();
                CleanPage();
                NewCurrentText();
                TextBoxPage.IsUndoEnabled = false; TextBoxPage.IsUndoEnabled = true;
            }
        }

        // Since files can be opened externally too, this functionality needs to be separated from the OpenFile function
        public string GetSelectionPath()
        {
            if (CheckUnsavedChanges())
            {
                OpenFileDialog openDialog = new OpenFileDialog() { Title = "Open file", Filter = "Text file " + SupportedFileFormats.TXT, FilterIndex = 1 };

                if (openDialog.ShowDialog() == true)
                    return PageFile.DocumentPath = openDialog.FileName;
            }
            return string.Empty;
        }

        public void OpenFile(string path)
        {
            try
            {
                if (path != string.Empty)
                {
                    StreamReader file = new StreamReader(path);
                    TextBoxPage.Text = file.ReadToEnd();
                    SetWindowTitle();
                    NewCurrentText();
                }
            }
            catch (Exception)
            {
                return;
            }
        }

        public bool SaveFile()
        {
            SaveFileDialog saveDialog = new SaveFileDialog() { Title = "Save file", Filter = "Text file " + SupportedFileFormats.TXT, FilterIndex = 1, AddExtension = true };
            saveDialog.ShowDialog();
            PageFile.DocumentPath = saveDialog.FileName;

            try
            {
                if (PageFile.DocumentPath != string.Empty)
                    using (StreamWriter file = new StreamWriter(PageFile.DocumentPath))
                    {
                        file.Write(TextBoxPage.Text);
                        SetWindowTitle();
                        NewCurrentText();
                        return true;
                    }
            }
            catch (Exception)
            {
                return false;
            }
            return false;
        }

        public void ZoomPage(float zoomValue)
        {
            TextBoxPage.FontSize = PageFile.DefaultFontSize * zoomValue;
        }

        public void CopyToClipboard()
        {
            if (TextBoxPage.Text != string.Empty)
                Clipboard.SetText(TextBoxPage.Text);
        }

        public void CleanPage()
        {
            TextBoxPage.Clear();
        }

        private void ButtonNewFile_Click(object sender, RoutedEventArgs e)
        {
            NewFile();
        }

        private void ButtonOpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFile(GetSelectionPath());
        }

        private void ButtonSaveFile_Click(object sender, RoutedEventArgs e)
        {
            SaveFile();
        }

        private void ButtonZoomIn_Click(object sender, RoutedEventArgs e)
        {
            PageFile.CurrentZoom += 0.1f; ZoomPage(PageFile.CurrentZoom);
        }

        private void ButtonZoomOut_Click(object sender, RoutedEventArgs e)
        {
            if (PageFile.CurrentZoom >= 0.2f)
                PageFile.CurrentZoom -= 0.1f; ZoomPage(PageFile.CurrentZoom);
        }

        private void ButtonClipboardCopy_Click(object sender, RoutedEventArgs e)
        {
            CopyToClipboard();
        }

        private void ButtonClean_Click(object sender, RoutedEventArgs e)
        {
            CleanPage();
        }

        // Event capture in case the file is opened externally
        void MainWindow_OnLoad(object sender, RoutedEventArgs e)
        {
            if (Application.Current.Properties["FilePath"] != null)
            {
                PageFile.DocumentPath = Application.Current.Properties["FilePath"].ToString();
                OpenFile(PageFile.DocumentPath);
            }
        }

        void MainWindow_OnClose(object sender, CancelEventArgs e)
        {
            if (!CheckUnsavedChanges())
            {
                e.Cancel = true;
            }
        }
    }
}
