using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Commands;
using Application.DTOs.AddressDTOs;
using Application.DTOs.UserDTOs;
using AutoMapper;
using Domain.Models;

namespace Application.Mappers
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            //User TO UserDTO
            CreateMap<User, UserDTO>();

            //CreateUserCommandDTO to USER -> To be returned
            CreateMap<CreateUserCommand, User>()
                .ForMember(dest => dest.AddressList, opt => opt.Ignore())
                .AfterMap((createUserCommand, user) =>
                {
                    user.AddressList = createUserCommand.AddressList
                        .Select(a => a.ToAddressFromAddressCreateDTO(user))
                        .ToList();
                });

            //Address to AddressDTO
            CreateMap<Address, AddressDTO>();

            //AddressCreateDTO to Address
            CreateMap<CreateAddressRequestDTO, Address>();
        }
    }
}