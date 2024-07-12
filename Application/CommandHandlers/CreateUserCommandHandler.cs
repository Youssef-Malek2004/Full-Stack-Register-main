using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Commands;
using Persistence.Data;
using Application.DTOs.UserDTOs;
using MediatR;
using Application.Mappers;
using System.ComponentModel.DataAnnotations;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Application.CommandHandlers
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserDTO>
    {
        private readonly ApplicationDBContext _context;

        public CreateUserCommandHandler(ApplicationDBContext context)
        {
            _context = context;
        }
        // public async Task<UserDTO> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        // {
        //     var userModel = request.ToUserFromUserCreateDTO();
        //     await validateUserFields(userModel);
        //     _context.Users.Add(userModel);
        //     await _context.SaveChangesAsync();
        //     return userModel.ToUserDTO();

        // }

        public async Task<UserDTO> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var userModel = request.ToUserFromUserCreateDTO();

            // Validate user fields
            await validateUserFields(userModel);

            await ValidateAddressFields(userModel, cancellationToken);

            foreach (var address in userModel.AddressList)
            {
                var governateUserCount = await _context.GovernateUserCounts.FindAsync(address.GovernateID);
                if (governateUserCount == null)
                {
                    governateUserCount = new GovernateUserCount
                    {
                        GovernateID = address.GovernateID,
                        UserCount = 1
                    };
                    _context.GovernateUserCounts.Add(governateUserCount);
                }
                else
                {
                    governateUserCount.UserCount += 1;
                    _context.GovernateUserCounts.Update(governateUserCount);
                }
            }

            // Add the user to the database
            _context.Users.Add(userModel);
            await _context.SaveChangesAsync(cancellationToken);

            return userModel.ToUserDTO();
        }

        private async Task ValidateAddressFields(User userModel, CancellationToken cancellationToken)
        {
            foreach (var address in userModel.AddressList)
            {
                var city = await _context.Cities.FindAsync([address.CityID], cancellationToken: cancellationToken) ?? throw new Exception($"The specified city with ID {address.CityID} does not exist.");


                _context.Entry(city).State = EntityState.Unchanged;
                address.City = city;

                // Check and attach the existing governate if necessary
                var governate = await _context.Governates.FindAsync([address.GovernateID], cancellationToken: cancellationToken) ?? throw new Exception($"The specified Governate with ID {address.GovernateID} does not exist.");
                _context.Entry(governate).State = EntityState.Unchanged;
                address.Governate = governate;
            }
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