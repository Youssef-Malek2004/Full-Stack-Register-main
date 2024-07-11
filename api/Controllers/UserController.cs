using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Application.Commands;
using Persistence.Data;
using Application.DTOs.UserDTOs;
using Application.Mappers;
using Domain.Models;
using Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MassTransit;
using Application.Contracts;

namespace api.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IPublishEndpoint _publishEndpoint;

        public UserController(IMediator mediator, IPublishEndpoint publishEndpoint)
        {
            _mediator = mediator;
            _publishEndpoint = publishEndpoint;
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

                await _publishEndpoint.Publish(new UserCreateResponseEvent
                {
                    UserModelDTO = createdUser,
                    ErrorModelDTO = new ErrorResponseDTO
                    {
                        Message = "",
                        Errors = ""
                    }
                });

                return CreatedAtAction(nameof(GetByID), new { id = createdUser.Id }, createdUser);
            }
            catch (ValidationException ex)
            {

                var errorResponse = new ErrorResponseDTO
                {
                    Message = "Validation failed",
                    Errors = ex.Message
                };

                await _publishEndpoint.Publish(new UserCreateResponseEvent
                {
                    UserModelDTO = null,
                    ErrorModelDTO = errorResponse
                });

                return BadRequest(errorResponse);
            }
        }

        private string validateUserFields(User userModel)
        {
            var validationContext = new ValidationContext(userModel);
            var validationResults = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
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