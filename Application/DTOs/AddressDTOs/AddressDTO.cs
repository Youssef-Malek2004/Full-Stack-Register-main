using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Models;

namespace Application.DTOs.AddressDTOs
{
    public class AddressDTO
    {
        public int? UserID { get; set; }
        public int Id { get; set; }
        public string GovernateID { get; set; } = string.Empty;
        public string CityID { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;
        public string BuildingNum { get; set; } = string.Empty;
        public int FlatNum { get; set; }
    }
}