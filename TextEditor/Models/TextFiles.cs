using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextEditor.Models
{
    class TextFiles
    {
        public int Id { get; set; }
        public Content Content { get; set; }
        public int PartNumber { get; set; }
        public string TextFile { get; set; }
    }
}
