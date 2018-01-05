using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BillZip.Dtos
{
    public class PatchUserDto
    {
        [Required]  
        public Guid Id { get; set; }

        public string UserName { get; set; }

        public Dictionary<string,string> Claims { get; set; }
    }
}
