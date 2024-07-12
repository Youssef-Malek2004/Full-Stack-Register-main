using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DTOs.TransferDTOs;
using MediatR;

namespace Application.Queries
{
    public class GetAllLookUpsQuery : IRequest<LookUpGetResponseDTO>
    {

    }
}