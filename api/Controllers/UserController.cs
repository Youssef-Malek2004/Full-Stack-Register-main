using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.DTOs.UserDTOs;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        public UserController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _context.Users.ToList()
            .Select(u => u.ToUserDTO()); //Select is just like the JS Map to map the users to the DTOs

            return Ok(users);
        }
        [HttpGet("{id}")]
        public IActionResult GetByID([FromRoute] int id)
        {
            var user = _context.Users.Find(id);

            if (user == null)
            {
                return NotFound(null);
            }

            return Ok(user.ToUserDTO());
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateUserRequestDTO UserDTO)
        {
            var userModel = UserDTO.ToUserFromUserCreateDTO();

            try
            {
                validateUserFields(userModel);
            }
            catch (ValidationException VException)
            {
                var errorResponse = new
                {
                    Message = "Validation failed",
                    Errors = VException.Message  // This assumes ValidationErrors is a List<string> in ValidationException
                };

                return BadRequest(errorResponse);
            }

            _context.Users.Add(userModel);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetByID), new { id = userModel.Id }, userModel.ToUserDTO());
        }

        private string validateUserFields(User userModel)
        {
            var validationContext = new ValidationContext(userModel);
            var validationResults = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(userModel, validationContext, validationResults, true);

            if (!isValid)
            {
                var errorMessages = string.Join("; ", validationResults.Select(r => r.ErrorMessage));
                throw new ValidationException(errorMessages);
            }
            return "Success";
        }
        /* JSON Working for post
   {
   "firstName": "Youssef",
   "middleName": "",
   "lastName": "Ahmed",
   "birthDate":"2024-01-07",
   "mobileNumber": "+548658561429",
   "email": "user@example.com",
   "addressList": [
       {
       "governate": "Cairo",
       "city": "Giza",
       "street": "Main Street",
       "buildingNum": "123",
       "flatNum": 5
       }
   ]
   }
*/
    }
}