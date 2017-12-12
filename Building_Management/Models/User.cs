using System;
using System.Collections.Generic;
using System.Text;

namespace Building_Management.Models
{
    public class User
    {
        public Guid id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
