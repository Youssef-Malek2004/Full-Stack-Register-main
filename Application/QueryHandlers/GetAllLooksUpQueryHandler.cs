using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DTOs.TransferDTOs;
using Application.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Application.QueryHandlers
{
    public class GetAllLooksUpQueryHandler(ApplicationDBContext context) : IRequestHandler<GetAllLookUpsQuery, LookUpGetResponseDTO>
    {
        private readonly ApplicationDBContext _context = context;

        async Task<LookUpGetResponseDTO> IRequestHandler<GetAllLookUpsQuery, LookUpGetResponseDTO>.Handle(GetAllLookUpsQuery request, CancellationToken cancellationToken)
        {
            var Governates = await _context.Governates.ToListAsync(cancellationToken);
            var Cities = await _context.Cities.ToListAsync(cancellationToken);
            return new LookUpGetResponseDTO
            {
                Governates = Governates,
                Cities = Cities
            };
        }
    }
}