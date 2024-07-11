using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DTOs.UserDTOs;

namespace Application.Contracts
{
    public record UserCreateEvent
    {
        public Guid CorrelationId { get; set; }
        public CreateUserRequestDTO UserModelDTO { get; set; } = new CreateUserRequestDTO();
    }
}