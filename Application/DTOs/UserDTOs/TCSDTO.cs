using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.DTOs.UserDTOs
{
    public class TCSDTO
    {
        public Guid CorrelationID { get; set; }
        public UserDTO? UserDTO { get; set; }
        public ErrorResponseDTO? ErrorResponseDTO { get; set; }

    }
}