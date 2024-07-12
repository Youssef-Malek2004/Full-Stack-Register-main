using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Application.Commands;
using Application.Contracts;
using Application.DTOs.UserDTOs;
using MassTransit;
using MediatR;

namespace api.Consumers
{
    public class UserCreateEventConsumer(IMediator mediator, IPublishEndpoint publishEndpoint) : IConsumer<UserCreateEvent>
    {
        private readonly IMediator _mediator = mediator;
        private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;

        public async Task Consume(ConsumeContext<UserCreateEvent> context)
        {
            try
            {
                var createdUser = await _mediator.Send(new CreateUserCommand(context.Message.UserModelDTO));

                await _publishEndpoint.Publish(new UserCreateResponseEvent
                {
                    CorrelationId = context.Message.CorrelationId,
                    IsSuccess = true,
                    UserModelDTO = createdUser,
                    ErrorModelDTO = new ErrorResponseDTO
                    {
                        Message = "",
                        Errors = ""
                    }
                });
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
                    CorrelationId = context.Message.CorrelationId,
                    IsSuccess = false,
                    UserModelDTO = null,
                    ErrorModelDTO = errorResponse
                });
            }
            catch (Exception ex)
            {
                var errorResponse = new ErrorResponseDTO
                {
                    Message = "User Creation failed",
                    Errors = ex.Message
                };
                await _publishEndpoint.Publish(new UserCreateResponseEvent
                {
                    CorrelationId = context.Message.CorrelationId,
                    IsSuccess = false,
                    UserModelDTO = null,
                    ErrorModelDTO = errorResponse
                });
            }
        }
    }
}