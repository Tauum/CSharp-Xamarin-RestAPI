﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GOVAPI.Models
{
    public class User
    {
        public int ID { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public int ScoreTotal { get; set; }
        public bool Admin { get; set; }
        public ICollection<UserProduct> UserProducts { get; set; } 
    }
}
