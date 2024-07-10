using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Persistence.Data;
using api.DTOs.UserDTOs;
using api.Mappers;
using api.Queries;
using MediatR;

namespace api.QueryHandlers
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDTO>
    {
        private readonly ApplicationDBContext _context;

        public GetUserByIdQueryHandler(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<UserDTO> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FindAsync([request.UserId], cancellationToken: cancellationToken);

            return user.ToUserDTO();
        }
    }
}