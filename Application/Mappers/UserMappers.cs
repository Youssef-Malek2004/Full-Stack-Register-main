using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Commands;
using Application.DTOs.UserDTOs;
using Domain.Models;

namespace Application.Mappers
{
    public static class UserMappers
    {
        public static UserDTO ToUserDTO(this User userModel)
        {
            return new UserDTO
            {
                Id = userModel.Id,
                FirstName = userModel.FirstName,
                MiddleName = userModel.MiddleName,
                LastName = userModel.LastName,
                BirthDate = userModel.BirthDate,
                MobileNumber = userModel.MobileNumber,
                Email = userModel.Email,
                // AddressList = userModel.AddressList,
            };
        }
        public static User ToUserFromUserCreateDTO(this CreateUserCommand userDTO)
        {
            User tempUser = new User
            {
                FirstName = userDTO.FirstName,
                MiddleName = userDTO.MiddleName,
                LastName = userDTO.LastName,
                BirthDate = userDTO.BirthDate,
                MobileNumber = userDTO.MobileNumber,
                Email = userDTO.Email,
            };
            tempUser.AddressList = userDTO.AddressList.Select(a => a.ToAddressFromAddressCreateDTO(tempUser)).ToList();
            return tempUser;
        }
    }
}