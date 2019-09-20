using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.IO;
using TextEditor.Models;

namespace TextEditor.Additional
{
    public static class Functions
    {
        public static void MyTextAlignment(RichTextBox rtb, TextAlignment textAlign)
        {
            try
            {
                var curBlock = FindBlockText(rtb);
                curBlock.TextAlignment = textAlign;
            }
            catch
            {

            }
        }

        public static void InitializeColorDialog(System.Windows.Forms.ColorDialog colorDlg)
        {
            colorDlg.AllowFullOpen = false;
            colorDlg.AnyColor = true;
            colorDlg.SolidColorOnly = false;
            colorDlg.Color = System.Drawing.Color.Black;
        }

        public static Block FindBlockText(RichTextBox rtb)
        {
            FlowDocument flowDoc = (FlowDocument)rtb.Document;
            BlockCollection bC = flowDoc.Blocks;
            var curCaret = rtb.CaretPosition;
            var curBlock = rtb.Document.Blocks.FirstOrDefault(x => x.ContentStart.CompareTo(curCaret) == -1 && x.ContentEnd.CompareTo(curCaret) == 1);

            return curBlock;
        }

        public static void OpenFile(RichTextBox rtb, String fileName)
        {
            try
            {
                FileDBContext db = new FileDBContext();
                var s = db.TextFiles.FirstOrDefault(temp => temp.Content.Name == fileName);

                FlowDocument doc = new FlowDocument();
                TextRange textRange = new TextRange(doc.ContentStart, doc.ContentEnd);

                if (s != null)
                    using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(s.TextFile)))
                    {
                        textRange.Load(stream, DataFormats.Rtf);
                        rtb.Document = doc;
                    }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static async Task SaveFile(RichTextBox rtb, String fileName)
        {
            if (CheckFileName(fileName))
                return;

            FileDBContext db = new FileDBContext();
            if (FileExist(fileName))
            {
                if (MessageBox.Show("Rewrite file?", "Question", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    RemoveFile(db, fileName);
                    goto m1;
                }
                else return;
            }

        m1:
            MemoryStream memoryStream = new MemoryStream();

            TextRange textRange = new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd);

            textRange.Save(memoryStream, System.Windows.DataFormats.Rtf);

            string textFile = Encoding.UTF8.GetString(memoryStream.ToArray());

            db.TextFiles.Add(new TextFiles
            {
                TextFile = textFile,
                PartNumber = 1,
                Content =
                new Content { CountPart = 1, Name = fileName, DateCreate = DateTime.Now, DateChange = DateTime.Now, Size = textFile.Length }
            });

            await db.SaveChangesAsync();
        }

        internal static bool RemoveFile(FileDBContext db, string fileName)
        {
            var content = db.Content.FirstOrDefault(x => x.Name == fileName);

            if (content != null)
            {
                try
                {
                    db.TextFiles.Remove((TextFiles)db.TextFiles.FirstOrDefault(x => x.Content.Id == content.Id));
                    db.Content.Remove(content);
                    return true;
                }
                catch { return false; }
            }
            else return true;
        }

        private static bool FileExist(string fileName)
        {
            using (FileDBContext db = new FileDBContext())
            {
                var temp = db.Content.FirstOrDefault(x => x.Name == fileName);

                return (temp == null ? false : true);
            }
        }

        private static bool CheckFileName(string fileName)
        {
            return String.IsNullOrEmpty(fileName);
        }
    }
}
