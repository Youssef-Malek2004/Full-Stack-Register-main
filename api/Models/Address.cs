using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class Address
    {
        [Required]
        public required User User { get; set; }

        public int Id { get; set; }

        [Required]
        public string Governate { get; set; } = string.Empty;

        [Required]
        public string City { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        public string Street { get; set; } = string.Empty;

        [Required]
        public string BuildingNum { get; set; } = string.Empty;

        [Required]
        public int FlatNum { get; set; }
    }
}