using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DTOs.UserDTOs;
using MediatR;

namespace Application.Queries
{
    public class GetAllUsersQuery : IRequest<List<UserDTO>>
    {

    }
}