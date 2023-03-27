using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeAssignment.Dtos.Analytics
{
    public class UserAssignmentDto
    {
        public int UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public int NumberOfDoneAssignments { get; set; }
    }
}