using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Contracts;
using Application.DTOs.UserDTOs;
using Application.Queries;
using MassTransit;
using MediatR;

namespace api.Consumers
{
    public class LookUpGetEventConsumer(IMediator mediator, IPublishEndpoint publishEndpoint) : IConsumer<LookUpGetEvent>
    {
        private readonly IMediator _mediator = mediator;
        private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;
        public async Task Consume(ConsumeContext<LookUpGetEvent> context)
        {
            try
            {
                var LookUps = await _mediator.Send(new GetAllLookUpsQuery());

                await _publishEndpoint.Publish(new LookUpGetResponseEvent
                {
                    CorrelationId = context.Message.CorrelationId,
                    LookUpModelDTO = LookUps,
                    ErrorResponseDTO = new ErrorResponseDTO
                    {
                        Message = "",
                        Errors = ""
                    }
                });
            }
            catch (Exception ex)
            {
                var errorResponse = new ErrorResponseDTO
                {
                    Message = "LookUps Get Failure",
                    Errors = ex.Message
                };
                await _publishEndpoint.Publish(new LookUpGetResponseEvent
                {
                    CorrelationId = context.Message.CorrelationId,
                    LookUpModelDTO = null,
                    ErrorResponseDTO = new ErrorResponseDTO
                    {
                        Message = "",
                        Errors = ""
                    }
                });
            }
        }
    }
}