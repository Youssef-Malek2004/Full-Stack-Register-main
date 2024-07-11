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

            while (true) //I dont know how to fix this for now -> Trying to get value that isnt there and then not consuming it ASK about it
            {
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
                    break;
                }
            }
        }

        public Task<UserDTO> WaitForResponse(Guid correlationId)
        {
            var tcs = new TaskCompletionSource<UserDTO>();
            lock (_responseTasks)
            {
                _responseTasks.Add(correlationId, tcs);
            }
            return tcs.Task;
        }
    }
}
