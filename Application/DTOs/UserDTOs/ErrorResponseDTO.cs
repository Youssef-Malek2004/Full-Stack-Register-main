using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.DTOs.UserDTOs
{
    public class ErrorResponseDTO
    {
        public string Message { get; set; } = string.Empty;
        public string Errors { get; set; } = string.Empty;
    }
}