namespace StudentDrive.Models
{
    using System;
    public class ShareHelperDto
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public float Size { get; set; }
        public bool Rewrite { get; set; }
        public bool Share { get; set; }
        public DateTime DateOfUpload { get; set; }

    }
}