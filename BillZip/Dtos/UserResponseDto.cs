using System;
using System.Collections.Generic;

namespace BillZip.Dtos
{
    public class UserResponseDto
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }        
        public DateTime LastLogin { get; set; }
        public DateTime DateCreated { get; set; }
        public Dictionary<string,string> Claims { get; set; }
    }
}
