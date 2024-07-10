using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DTOs.AddressDTOs;
using MediatR;

namespace Application.Queries
{
    public class GetAllAddressesQuery : IRequest<List<AddressDTO>>
    {

    }
}