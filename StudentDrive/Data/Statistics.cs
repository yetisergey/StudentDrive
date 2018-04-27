namespace Data
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    public class Statistics
    {
        public Statistics() { }
        public Statistics(int _uploadCount, int _downloadCount, int _shareCount, Guid userId)
        {
            Id = Guid.NewGuid();
            UploadCount = _uploadCount;
            DownloadCount = _downloadCount;
            ShareCount = _shareCount;
            UserId = userId;
        }
        [Key]
        public Guid Id { get; set; }
        [Required]
        public int UploadCount { get; set; }
        [Required]
        public int DownloadCount { get; set; }
        [Required]
        public int ShareCount { get; set; }
        [Required]
        public Guid UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}