using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Persistence.Data;
using Application.DTOs.UserDTOs;
using Application.Mappers;
using Application.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace Application.QueryHandlers
{
    public class GetAllUsersQueryHandler(ApplicationDBContext context, IMapper mapper) : IRequestHandler<GetAllUsersQuery, List<UserDTO>>
    {
        private readonly ApplicationDBContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<List<UserDTO>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _context.Users
                              .ToListAsync(cancellationToken);

            var userDTOs = users.Select(u => _mapper.Map<UserDTO>(u)).ToList();

            return userDTOs;
        }
    }
}