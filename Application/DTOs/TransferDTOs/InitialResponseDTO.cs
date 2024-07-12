using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.DTOs.TransferDTOs
{
    public class InitialResponseDTO
    {
        public Guid CorrelationID { get; set; }
        public string initialResponse { get; set; } = string.Empty;
    }
}