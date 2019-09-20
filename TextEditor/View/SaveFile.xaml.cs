using System;
using System.Data.Entity;
using System.Windows;
using TextEditor.Models;

namespace TextEditor
{
    /// <summary>
    /// Interaction logic for OpenFile.xaml
    /// </summary>
    public partial class SaveFile : Window
    {
        public SaveFile()
        {
            InitializeComponent();

            using (FileDBContext db = new FileDBContext())
            {
                db.Content.Load();

                filesGridSave.ItemsSource = db.Content.Local.ToBindingList();
            }
        }

        public String fileName
        {
            get
            {
                if (!String.IsNullOrEmpty(tbFileName.Text.ToString()))
                    return tbFileName.Text.ToString() + ".file";
                else return String.Empty;
            }
        }

        private void BtSaveFile_Click(object sender, RoutedEventArgs e)
        {                       
            this.DialogResult = true;
        }

        private void BtCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
