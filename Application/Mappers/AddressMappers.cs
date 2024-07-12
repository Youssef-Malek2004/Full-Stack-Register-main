using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DTOs.AddressDTOs;
using Domain.Models;

namespace Application.Mappers
{
    public static class AddressMappers
    {
        public static AddressDTO ToAddressDTO(this Address addressModel)
        {
            return new AddressDTO
            {
                Id = addressModel.Id,
                UserID = addressModel.UserID,
                GovernateID = addressModel.GovernateID,
                CityID = addressModel.CityID,
                Street = addressModel.Street,
                BuildingNum = addressModel.BuildingNum,
                FlatNum = addressModel.FlatNum,
            };
        }
        public static Address ToAddressFromAddressCreateDTO(this CreateAddressRequestDTO addressModel, User user)
        {
            return new Address
            {
                User = user,
                GovernateID = addressModel.Governate,
                CityID = addressModel.City,
                Street = addressModel.Street,
                BuildingNum = addressModel.BuildingNum,
                FlatNum = addressModel.FlatNum,
            };
        }
    }
}