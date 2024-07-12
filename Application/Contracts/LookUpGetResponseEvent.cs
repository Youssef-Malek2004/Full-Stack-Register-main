using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DTOs.TransferDTOs;
using Application.DTOs.UserDTOs;

namespace Application.Contracts
{
    public class LookUpGetResponseEvent
    {
        public Guid CorrelationId { get; set; }
        public LookUpGetResponseDTO? LookUpModelDTO { get; set; }
        public ErrorResponseDTO? ErrorResponseDTO { get; set; }
    }
}