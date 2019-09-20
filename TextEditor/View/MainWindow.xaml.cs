using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.IO;
using System.Windows.Threading;
using Microsoft.Win32;
using TextEditor.Additional;

namespace TextEditor.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer;

        String fileName;

        public MainWindow()
        {
            InitializeComponent();

            cbFontStyle.ItemsSource = Fonts.SystemFontFamilies.OrderBy(f => f.Source);
            cbFontSize.ItemsSource = new List<double>() { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };
            cbLineHeight.ItemsSource = new List<double>() { 1, 1.5, 2, 2.5, 3, 3.5, 4 };
        }

        private void BtnOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenFile openFile = new OpenFile();

            if (openFile.ShowDialog() == true)
            {
                if (!String.IsNullOrEmpty(new TextRange(rtbTextEditor.Document.ContentStart, rtbTextEditor.Document.ContentEnd).Text))                
                    if (MessageBox.Show("Save current file?", "Question", MessageBoxButton.YesNo) == MessageBoxResult.Yes)                    
                        BtnSave_Click(sender, e);           

                Functions.OpenFile(rtbTextEditor, openFile.fileName);
                fileName = openFile.fileName;
            }
        }

        private void CbFontStyle_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbFontStyle.SelectedItem != null)
                rtbTextEditor.Selection.ApplyPropertyValue(Inline.FontFamilyProperty, cbFontStyle.SelectedItem);
        }

        private void CbFontSize_TextChanged(object sender, TextChangedEventArgs e)
        {
            rtbTextEditor.Selection.ApplyPropertyValue(Inline.FontSizeProperty, (double)cbFontSize.SelectedValue * 2);
        }

        private void CbLineHeight_TextChanged(object sender, TextChangedEventArgs e)
        {
            var curBlock = Functions.FindBlockText(rtbTextEditor);
            double temp;
            if (double.TryParse(cbLineHeight.Text, out temp))
                curBlock.LineHeight = temp;
        }

        private void RtbTextEditor_SelectionChanged(object sender, RoutedEventArgs e)
        {
            object temp = rtbTextEditor.Selection.GetPropertyValue(Inline.FontWeightProperty);
            btnBold.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(FontWeights.Bold));

            temp = rtbTextEditor.Selection.GetPropertyValue(Inline.FontStyleProperty);
            btnItalics.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(FontStyles.Italic));

            temp = rtbTextEditor.Selection.GetPropertyValue(Inline.TextDecorationsProperty);
            btnUnderline.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(TextDecorations.Underline));

            temp = rtbTextEditor.Selection.GetPropertyValue(Inline.FontFamilyProperty);
            cbFontStyle.SelectedItem = temp;

            temp = rtbTextEditor.Selection.GetPropertyValue(Inline.FontSizeProperty);
            cbFontSize.Text = temp.ToString();
        }

        private void BtnLeftAlignment_Click(object sender, RoutedEventArgs e)
        {
            Functions.MyTextAlignment(rtbTextEditor, TextAlignment.Left);
        }

        private void BtnRightAlignment_Click(object sender, RoutedEventArgs e)
        {
            Functions.MyTextAlignment(rtbTextEditor, TextAlignment.Right);
        }

        private void BtnCenterAlignment_Click(object sender, RoutedEventArgs e)
        {
            Functions.MyTextAlignment(rtbTextEditor, TextAlignment.Center);
        }

        private void BtnWidthAlignment_Click(object sender, RoutedEventArgs e)
        {
            Functions.MyTextAlignment(rtbTextEditor, TextAlignment.Justify);
        }

        private void BtnRedo_Click(object sender, RoutedEventArgs e)
        {
            rtbTextEditor.Redo();
        }

        private void BtnUndo_Click(object sender, RoutedEventArgs e)
        {
            rtbTextEditor.Undo();
        }

        private void BtnTextClrChng_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.ColorDialog colorDlg = new System.Windows.Forms.ColorDialog();

            if (colorDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    rtbTextEditor.Selection.ApplyPropertyValue(Inline.ForegroundProperty, new SolidColorBrush(Color.FromArgb(colorDlg.Color.A, colorDlg.Color.R, colorDlg.Color.G, colorDlg.Color.B)));
                }
                catch
                {

                }
            }
        }

        private void BtnBckgrClrChng_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.ColorDialog colorDlg = new System.Windows.Forms.ColorDialog();

            if (colorDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    rtbTextEditor.Selection.ApplyPropertyValue(Inline.BackgroundProperty, new SolidColorBrush(Color.FromArgb(colorDlg.Color.A, colorDlg.Color.R, colorDlg.Color.G, colorDlg.Color.B)));
                }
                catch
                {

                }
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            SaveFile saveFile = new SaveFile();

            if (saveFile.ShowDialog() == true)
            {
                Functions.SaveFile(rtbTextEditor, saveFile.fileName);
                fileName = saveFile.fileName;
                MessageBox.Show("File saved");
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {            
            timer = new DispatcherTimer();
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = new TimeSpan(0, 5, 0);
            timer.Start();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            TextRange tr = new TextRange(rtbTextEditor.Document.ContentStart, rtbTextEditor.Document.ContentEnd);
            string str = tr.Text;

            if (!String.IsNullOrEmpty(fileName))
                Functions.SaveFile(rtbTextEditor, fileName);
        }

        private void timerHighLight_Tick(object sender, EventArgs e)
        {
            TextRange tr = new TextRange(rtbTextEditor.Document.ContentStart, rtbTextEditor.Document.ContentEnd);
            string str = tr.Text;
            rtbTextEditor.AppendText(HighlightJSON.SyntaxHighlightJson(str));
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            timer.Tick -= timer_Tick;            
        }

        private void BtnSaveToFile_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "RTF (.rtf)|*.rtf|(*.txt)|*.txt|All files (*.*)|*.*";

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    fileName = saveFileDialog.FileName;
                    var range = new TextRange(rtbTextEditor.Document.ContentStart, rtbTextEditor.Document.ContentEnd);
                    var fStream = new FileStream(fileName, FileMode.Create);
                    range.Save(fStream, DataFormats.Rtf);
                    fStream.Close();
                    MessageBox.Show("File saved");
                }
                catch { MessageBox.Show("Not right format"); }
            }
        }

        private void BtnOpenFromFile_Click(object sender, RoutedEventArgs e)
        {            
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "RTF (.rtf)|*.rtf|(*.txt)|*.txt|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    if (File.Exists(openFileDialog.FileName))
                    {
                        var range = new TextRange(rtbTextEditor.Document.ContentStart, rtbTextEditor.Document.ContentEnd);
                        var fStream = new FileStream(openFileDialog.FileName, FileMode.OpenOrCreate);
                        range.Load(fStream, DataFormats.Rtf);
                        fStream.Close();
                    }
                    fileName = openFileDialog.FileName;
                }
                catch (Exception ex)
                { MessageBox.Show(ex.Message); }
            }            
        }
    }
}
