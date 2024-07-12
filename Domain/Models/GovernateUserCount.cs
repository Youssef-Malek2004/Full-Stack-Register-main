using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class GovernateUserCount
    {
        [Key]
        public string GovernateID { get; set; } = string.Empty;
        public int UserCount { get; set; }
    }
}