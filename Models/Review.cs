using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GOVAPI.Models
{
    public class Review
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public User User { get; set; } // foreign key  this points to the user but is not used
        public int ProductID { get; set; }
        public Product Product { get; set; } // foreign key  this points to the product but is not used
        public string Description { get; set; }
        public bool Visible { get; set; }
    }
}
