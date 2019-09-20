using System;
using System.Collections.Generic;
using System.Data.Entity;
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
using TextEditor.Models;

namespace TextEditor
{
    /// <summary>
    /// Interaction logic for OpenFile.xaml
    /// </summary>
    public partial class OpenFile : Window
    {
        public OpenFile()
        {
            InitializeComponent();

            using (FileDBContext db = new FileDBContext())
            {
                db.Content.Load();

                filesGrid.ItemsSource = db.Content.Local.ToBindingList();
            }
        }

        public String fileName
        {
            get
            {
                try
                {
                    if (!String.IsNullOrEmpty((filesGrid.SelectedItem as Content).Name))
                        return (filesGrid.SelectedItem as Content).Name;                    
                }
                catch { MessageBox.Show("Not chosen file"); }
                return null;
            }
        }

        private void BtOpenFile_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void BtCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void LbFiles_Initialized(object sender, EventArgs e)
        {

        }
    }
}
