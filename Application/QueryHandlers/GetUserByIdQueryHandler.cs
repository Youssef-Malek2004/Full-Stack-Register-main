using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Persistence.Data;
using Application.DTOs.UserDTOs;
using Application.Mappers;
using Application.Queries;
using MediatR;
using AutoMapper;

namespace Application.QueryHandlers
{
    public class GetUserByIdQueryHandler(ApplicationDBContext context, IMapper mapper) : IRequestHandler<GetUserByIdQuery, UserDTO>
    {
        private readonly ApplicationDBContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<UserDTO> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FindAsync([request.UserId], cancellationToken: cancellationToken);

            return _mapper.Map<UserDTO>(user);
        }
    }
}