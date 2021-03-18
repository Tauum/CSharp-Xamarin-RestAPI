using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GOVAPI.Models
{
    public class Product
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int ReleaseYear { get; set; }
        public string Description { get; set; }
        public int Score { get; set; }
        public string PRef { get; set; }
        public int? ImageId { get; set; }  //this ? means it can also be a null value
        public virtual Image Image { get; set; } // foreign key  this points to the image but is not used
        public int? CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
