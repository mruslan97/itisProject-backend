using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore.Models.Entities
{
    public class Message
    {
        public int Id { get; set; }
        public AppUser From { get; set; }
        public AppUser To { get; set; }
        public string Text { get; set; }
        public bool IsReaded { get; set; }
    }
}
