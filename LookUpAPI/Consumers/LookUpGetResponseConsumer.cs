using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Contracts;
using Application.DTOs.TransferDTOs;
using Application.Exceptions;
using Application.Queries;
using MassTransit;

namespace LookUpAPI.Consumers
{
    public class LookUpGetResponseConsumer(Dictionary<Guid, TaskCompletionSource<LookUpGetResponseDTO>> responseTasks) : IConsumer<LookUpGetResponseEvent>
    {
        private readonly Dictionary<Guid, TaskCompletionSource<LookUpGetResponseDTO>> _responseTasks = responseTasks;

        public async Task Consume(ConsumeContext<LookUpGetResponseEvent> context)
        {
            var responseEvent = context.Message;

            if (_responseTasks.TryGetValue(responseEvent.CorrelationId, out var tcs))
            {
                if (responseEvent.LookUpModelDTO is not null)
                {
                    tcs.SetResult(new LookUpGetResponseDTO
                    {
                        CorrelationId = responseEvent.CorrelationId,
                        Governates = responseEvent.LookUpModelDTO.Governates,
                        Cities = responseEvent.LookUpModelDTO.Cities
                    });
                }
                else
                {
                    tcs.SetException(new LookUpsGetException($"User creation failed: {responseEvent.ErrorResponseDTO.Errors}", responseEvent.ErrorResponseDTO));
                }
            }
        }
    }
}