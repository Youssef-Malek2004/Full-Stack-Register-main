using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Models;

namespace Application.DTOs.TransferDTOs
{
    public class LookUpGetResponseDTO
    {
        public Guid CorrelationId { get; set; }
        public List<Governate> Governates { get; set; } = [];
        public List<City> Cities { get; set; } = [];
    }
}