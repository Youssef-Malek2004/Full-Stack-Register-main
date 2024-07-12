using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Domain.Models
{
    public class Address
    {
        public User? User { get; set; } //NavProperty for the User
        public Governate? Governate { get; set; }
        public City? City { get; set; }

        public int Id { get; set; }

        public int? UserID { get; set; }

        [Required]
        public string GovernateID { get; set; } = string.Empty;

        [Required]
        public string CityID { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        public string Street { get; set; } = string.Empty;

        [Required]
        public string BuildingNum { get; set; } = string.Empty;

        [Required]
        public int FlatNum { get; set; }
    }
}