namespace Data
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    public class Share
    {
        public Share() { }
        public Share(User owner, User toUser, File file)
        {
            Id = Guid.NewGuid();
            OwnerId = owner.Id;
            ToUserId = toUser.Id;
            FileId = file.Id;
        }

        [Key]
        public Guid Id { get; set; }
        [Required, ForeignKey("Owner")]
        public Guid OwnerId { get; set; }
        [Required, ForeignKey("ToUser")]
        public Guid ToUserId { get; set; }
        [Required]
        public Guid FileId { get; set; }
        [Required]
        public Boolean IsWrite { get; set; }
        [ForeignKey("OwnerId")]
        public virtual User Owner { get; set; }
        [ForeignKey("ToUserId")]
        public virtual User ToUser { get; set; }
        [ForeignKey("FileId")]
        public virtual File File { get; set; }
    }
}