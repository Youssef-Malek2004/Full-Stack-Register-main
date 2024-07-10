using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using api.Commands;
using api.Data;
using api.DTOs.UserDTOs;
using api.Mappers;
using Domain.Models;
using api.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _mediator.Send(new GetAllUsersQuery());
            return Ok(users);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByID([FromRoute] int id)
        {
            var user = await _mediator.Send(new GetUserByIdQuery { UserId = id });

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserRequestDTO userDTO)
        {
            try
            {
                var createdUser = await _mediator.Send(new CreateUserCommand(userDTO));

                return CreatedAtAction(nameof(GetByID), new { id = createdUser.Id }, createdUser);
            }
            catch (ValidationException ex)
            {
                var errorResponse = new
                {
                    Message = "Validation failed",
                    Errors = ex.Message
                };
                return BadRequest(errorResponse);
            }
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