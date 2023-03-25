using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeAssignment.Domain.Enums;

namespace HomeAssignment.Dtos.Assignment
{
    public class CreateAssignmentDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Status Status { get; set; } = Status.Pending;
        public Importance Importance { get; set; } = Importance.Low;
        public int UserId { get; set; }
        // public User? AssignedToUser { get; set; }
        public DateTime DueDate { get; set; } = DateTime.Now.AddDays(7);
    }
}