using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore.Models.Entities
{
    public class Tariff
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Internet { get; set; }
        public int Calls { get; set; }
        public int Sms { get; set; }
        public int Cost { get; set; }
    }
}
