using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab1.Models
{
    public enum DocumentType
    {
        Application,
        Vacation,
        Firing
    }

    public class Document
    {
        public int Id { get; set; }

        public DocumentType Type { get; set; }

        public string Content { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Signed { get; set; }

        public string Author { get; set; }
    }
}
