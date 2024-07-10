using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs.UserDTOs;
using MediatR;

namespace api.Queries
{
    public class GetUserByIdQuery : IRequest<UserDTO>
    {
        public int UserId { get; set; }
    }
}