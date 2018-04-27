namespace Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    public class File
    {
        public File()
        { }
        public File(User user, string name, string path, float size, byte[] fileSource)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(path))
            {
                Id = Guid.NewGuid();
                UserId = user.Id;
                Name = name;
                Size = size;
                FileSource = fileSource;
                IsDeleted = false;
            }
        }
        [Key]
        public Guid Id { get; set; }
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public string Name { get; set; }
        //[Required]
        //public string Path { get; set; }
        [Required]
        public float Size { get; set; }
        [Required]
        public byte[] FileSource { get; set; }
        [Required]
        public bool IsDeleted { get; set; }
        [Required]
        public DateTime DateOfUpload { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        public virtual ICollection<Share> Shares { get; set; }
    }
}