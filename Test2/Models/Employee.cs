namespace Test2.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Position { get; set; }
        public int Age { get; set; }
        public required string Address { get; set; }
        public required string ShiftTiming { get; set; }
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
    }
}

