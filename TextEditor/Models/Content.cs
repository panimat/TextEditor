using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextEditor.Models
{
    public class Content
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CountPart { get; set; }
        public DateTime DateCreate { get; set; }
        public DateTime DateChange { get; set; }
        public int Size { get; set; }
    }
}
