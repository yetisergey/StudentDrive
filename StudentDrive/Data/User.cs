namespace Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    public class User
    {               
        [Key]
        public Guid Id { get; set; }
        [Required, MaxLength(255)]
        public string FirstName { get; set; }
        [Required]
        public string SecondName { get; set; }
        [Required]
        public string Login { get; set; }
        [Required]
        public Guid Password { get; set; }
        [Required]
        public float Space = 262144000;
        public string VkId { get; set; }
        [Required]
        public float DiscUsage { get; set; }
        public virtual ICollection<File> Files { get; set; }
        [InverseProperty("Owner")]
        public virtual ICollection<Share> MyShares { get; set; }
        [InverseProperty("ToUser")]
        public virtual ICollection<Share> FriendShares { get; set; }
    }
}
