using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DTOs.UserDTOs;

namespace Application.Exceptions
{
    public class LookUpsGetException : Exception
    {
        public ErrorResponseDTO ErrorModel { get; }

        public LookUpsGetException(string message, ErrorResponseDTO errorModel)
            : base(message)
        {
            ErrorModel = errorModel;
        }
    }
}