using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BF1.Models
{
    public class File
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Path { get; set; }

        public int StudentId { get; set; }
    }
}