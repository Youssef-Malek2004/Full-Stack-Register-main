using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Contracts;
using Application.DTOs.UserDTOs;
using Application.Exceptions;
using MassTransit;

namespace GatewayAPI.Consumers
{
    public class UserCreateResponseConsumer : IConsumer<UserCreateResponseEvent>
    {
        private readonly Dictionary<Guid, TaskCompletionSource<TCSDTO>> _responseTasks;

        public UserCreateResponseConsumer(Dictionary<Guid, TaskCompletionSource<TCSDTO>> responseTasks)
        {
            _responseTasks = responseTasks;
        }

        public async Task Consume(ConsumeContext<UserCreateResponseEvent> context)
        {

            var responseEvent = context.Message;

            if (_responseTasks.TryGetValue(responseEvent.CorrelationId, out var tcs))
            {
                if (responseEvent.IsSuccess)
                {
                    tcs.SetResult(new TCSDTO
                    {
                        CorrelationID = responseEvent.CorrelationId,
                        UserDTO = responseEvent.UserModelDTO,
                        ErrorResponseDTO = responseEvent.ErrorModelDTO
                    });
                }
                else
                {
                    tcs.SetException(new UserCreationException($"User creation failed: {responseEvent.ErrorModelDTO.Errors}", responseEvent.ErrorModelDTO));
                }
            }
        }
    }
}
