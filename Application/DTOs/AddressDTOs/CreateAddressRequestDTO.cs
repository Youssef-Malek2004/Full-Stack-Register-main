using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.DTOs.AddressDTOs
{
    public class CreateAddressRequestDTO
    {
        public string Governate { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;
        public string BuildingNum { get; set; } = string.Empty;
        public int FlatNum { get; set; }
    }
}