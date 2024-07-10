using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Persistence.Data;
using api.DTOs.UserDTOs;
using api.Mappers;
using api.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace api.QueryHandlers
{
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, List<UserDTO>>
    {
        private readonly ApplicationDBContext _context;

        public GetAllUsersQueryHandler(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<List<UserDTO>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _context.Users
                              .ToListAsync(cancellationToken);

            var userDTOs = users.Select(u => u.ToUserDTO()).ToList();

            return userDTOs;
        }
    }
}