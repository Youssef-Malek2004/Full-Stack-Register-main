using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs.UserDTOs;
using Domain.Models;
using MediatR;

namespace api.Commands
{
    public class CreateUserCommand : IRequest<UserDTO>
    {
        public string FirstName { get; set; } = string.Empty;
        public string MiddleName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateOnly BirthDate { get; set; }
        public string MobileNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public IList<Address> AddressList { get; set; } = [];

        // We have a constructor to pass the data immediately from the CreateUserRequestDTO in the Controller + no need to make the controller look ugly
        public CreateUserCommand(CreateUserRequestDTO userDTO)
        {
            FirstName = userDTO.FirstName;
            MiddleName = userDTO.MiddleName;
            LastName = userDTO.LastName;
            BirthDate = userDTO.BirthDate;
            MobileNumber = userDTO.MobileNumber;
            Email = userDTO.Email;
            AddressList = userDTO.AddressList;
        }

        public CreateUserCommand()
        {
        }
    }
}