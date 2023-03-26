using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeAssignment.Domain
{
    public class User
    {
        public int Id { get; set; } 
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public List<Assignment>? assignments { get; set; }
        public byte[]? PasswordHash { get; set; }
        public byte[]? PasswordSalt { get; set; }
    }
}