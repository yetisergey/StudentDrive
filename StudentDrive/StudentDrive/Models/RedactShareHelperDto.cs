namespace StudentDrive.Models
{
    using System;
    public class RedactShareHelperDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public bool checkedShare { get; set; }
    }
}