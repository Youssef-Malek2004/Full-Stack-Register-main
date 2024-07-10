using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Commands;
using api.Data;
using api.DTOs.UserDTOs;
using MediatR;
using api.Mappers;
using System.ComponentModel.DataAnnotations;
using Domain.Models;

namespace api.CommandHandlers
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserDTO>
    {
        private readonly ApplicationDBContext _context;

        public CreateUserCommandHandler(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<UserDTO> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var userModel = request.ToUserFromUserCreateDTO();
            await validateUserFields(userModel);
            _context.Users.Add(userModel);
            await _context.SaveChangesAsync();
            return userModel.ToUserDTO();

        }

        private async Task validateUserFields(User userModel)
        {
            var validationContext = new ValidationContext(userModel);
            var validationResults = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(userModel, validationContext, validationResults, true);

            if (!isValid)
            {
                var errorMessages = string.Join("; ", validationResults.Select(r => r.ErrorMessage));
                throw new ValidationException(errorMessages);
            }
        }
    }
}