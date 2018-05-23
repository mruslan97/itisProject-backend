using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore.Models.Entities
{
    public class Dialogue
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public AppUser From { get; set; }
        public AppUser To { get; set; }
    }
}
