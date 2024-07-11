using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Application.DTOs.AddressDTOs;
using Domain.Models;

namespace Application.DTOs.UserDTOs
{
    public class CreateUserRequestDTO
    {
        public string FirstName { get; set; } = string.Empty;
        public string MiddleName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateOnly BirthDate { get; set; }
        public string MobileNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public IList<CreateAddressRequestDTO> AddressList { get; set; } = []; //MUST BE FIXED it doesnt take addressLists it takes the DTOS FINALLY GOT IT

        public CreateUserRequestDTO()
        {

        }
    }
}