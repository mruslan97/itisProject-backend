using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore.Models.Entities
{
    public class Message
    {
        public int Id { get; set; }
        public AppUser FromUser { get; set; }
        public string Text { get; set; }
        public bool IsReaded { get; set; }
        public DateTime DateTime { get; set; }
        public Dialogue Dialogue { get; set; }
    }
}
