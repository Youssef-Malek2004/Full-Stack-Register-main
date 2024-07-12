using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DTOs.AddressDTOs;
using Application.Mappers;
using Application.Queries;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Application.QueryHandlers
{
    public class GetAllAddressesQueryHandler : IRequestHandler<GetAllAddressesQuery, List<AddressDTO>>
    {
        private readonly ApplicationDBContext _context;
        private readonly IMapper _mapper;

        public GetAllAddressesQueryHandler(ApplicationDBContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }
        public async Task<List<AddressDTO>> Handle(GetAllAddressesQuery request, CancellationToken cancellationToken)
        {
            var addresses = await _context.Addresses
                              .ToListAsync(cancellationToken);

            var AddressDTOs = addresses.Select(u => _mapper.Map<AddressDTO>(u)).ToList();

            return AddressDTOs;
        }
    }
}