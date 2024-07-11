using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DTOs.UserDTOs;

namespace Application.Contracts
{
    public class UserCreateResponseEvent
    {
        public Guid CorrelationId { get; set; }
        public bool IsSuccess { get; set; }
        public UserDTO? UserModelDTO { get; set; }

        public ErrorResponseDTO? ErrorModelDTO { get; set; }

    }
}