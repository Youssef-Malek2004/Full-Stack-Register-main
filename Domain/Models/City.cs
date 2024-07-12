using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class City
    {
        [Required]
        public string CityID { get; set; } = string.Empty;
    }
}