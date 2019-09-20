using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace TextEditor.Models
{
    class FileDBContext : DbContext
    {

        public FileDBContext() : base("DefaultConnection")
        {

        }

        public DbSet<Content> Content { get; set; }
        public DbSet<TextFiles> TextFiles { get; set; }
    }
}
