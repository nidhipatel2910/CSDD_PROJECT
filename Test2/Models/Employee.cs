using System.ComponentModel.DataAnnotations;

namespace Test2.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Position { get; set; }
        public int Age { get; set; }
        public required string Address { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public required string Email { get; set; }

        public required string ShiftTimings { get; set; }

        public bool InvitationAccepted { get; set; } = false;

        public Employee()
        {
            InvitationAccepted = false;
        }

    }
}

