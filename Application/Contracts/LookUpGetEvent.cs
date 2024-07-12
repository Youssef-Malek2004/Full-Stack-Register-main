using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DTOs.TransferDTOs;

namespace Application.Contracts
{
    public class LookUpGetEvent
    {
        public Guid CorrelationId { get; set; }
    }
}