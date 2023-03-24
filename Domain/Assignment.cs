using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeAssignment.Domain.Enums;

namespace HomeAssignment.Domain
{
    public class Assignment
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Status Status { get; set; } = Status.Pending;
        public Importance Importance { get; set; } = Importance.Low;
        public int UserId { get; set; }
        // public User? AssignedToUser { get; set; }
        //Add time created and due date

    }
}