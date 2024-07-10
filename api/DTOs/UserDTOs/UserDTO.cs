using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace api.DTOs.UserDTOs
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string MiddleName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateOnly BirthDate { get; set; }
        public string MobileNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        // public IList<Address> AddressList { get; set; } = []; //This makes a cycled dependancy

    }
}