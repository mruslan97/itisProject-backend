using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace AspNetCore.Models.Entities
{
    public class Customer
    {
        public int Id { get; set; }
        public string IdentityId { get; set; }
        public AppUser Identity { get; set; }
        public int Balance { get; set; }
        public string PassportSeries { get; set; }
        public int RepublicId { get; set; }
        public Republic Republic { get; set; }
        public Tariff Tariff { get; set; }
    }
}