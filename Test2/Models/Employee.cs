using System;
using System.ComponentModel.DataAnnotations;

namespace Test2.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Position is required")]
        public string Position { get; set; }

        public int Age { get; set; }

        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Shift Timings are required")]
        public string ShiftTimings { get; set; }

        public bool InvitationAccepted { get; set; }

        public Employee()
        {
            InvitationAccepted = false;
        }
    }

    public class InvitationHistory
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "Event Type is required")]
        public string EventType { get; set; }

        public DateTime EventDateTime { get; set; }
    }
}
