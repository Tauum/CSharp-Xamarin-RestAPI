using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GOVAPI.Models
{
    public class Image
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public byte[] Information { get; set; }
        public string TypeUsed { get; set; }
        public DateTime DateChanged { get; set; }
        public string Extension { get => Path.GetExtension(this.Name); }
        
        public Image()
        {
            this.DateChanged = DateTime.Now;
        }
    }
}
