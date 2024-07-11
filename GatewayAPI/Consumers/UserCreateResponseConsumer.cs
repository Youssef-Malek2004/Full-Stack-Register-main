using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Contracts;
using Application.DTOs.UserDTOs;
using MassTransit;

namespace GatewayAPI.Consumers
{
    public class UserCreateResponseConsumer : IConsumer<UserCreateResponseEvent>
    {
        private readonly Dictionary<Guid, TaskCompletionSource<UserDTO>> _responseTasks;

        public UserCreateResponseConsumer()
        {
            _responseTasks = new Dictionary<Guid, TaskCompletionSource<UserDTO>>();
        }

        public async Task Consume(ConsumeContext<UserCreateResponseEvent> context)
        {
            var responseEvent = context.Message;

            if (_responseTasks.TryGetValue(responseEvent.CorrelationId, out var tcs))
            {
                if (responseEvent.IsSuccess)
                {
                    tcs.SetResult(responseEvent.UserModelDTO);
                }
                else
                {
                    tcs.SetException(new Exception($"User creation failed: {responseEvent.ErrorModelDTO.Errors}"));
                }

                _responseTasks.Remove(responseEvent.CorrelationId);
            }
        }

        public Task<UserDTO> WaitForResponse(Guid correlationId)
        {
            var tcs = new TaskCompletionSource<UserDTO>();
            _responseTasks.Add(correlationId, tcs);
            return tcs.Task;
        }
    }
}
