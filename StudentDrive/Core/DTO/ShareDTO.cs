namespace Core.DTO
{
    using Data;
    using System;
    public class ShareDTO
    {
        public ShareDTO() { }
        public ShareDTO(Share share)
        {
            Id = share.Id;
            OwnerId = share.OwnerId;
            ToUserId = share.ToUserId;
            FileId = share.FileId;
            IsWrite = share.IsWrite;
        }

        public Guid? Id { get; set; }
        public Guid OwnerId { get; set; }
        public Guid ToUserId { get; set; }
        public Guid FileId { get; set; }
        [System.ComponentModel.DefaultValue(false)]
        public bool IsWrite { get; set; }

        public Share Map(ShareDTO share)
        {
            Share sh = new Share();
            sh.Id = share.Id ?? Guid.NewGuid();
            if (share.OwnerId == null || share.ToUserId == null || share.FileId == null)
                throw new Exception("Внутреняя ошибка сервера");
            sh.OwnerId = share.OwnerId;
            sh.ToUserId = share.ToUserId;
            sh.FileId = share.FileId;
            sh.IsWrite = share.IsWrite;
            return sh;
        }
        public Share Map()
        {
            Share sh = new Share();
            sh.Id = this.Id ?? Guid.NewGuid();
            if (this.OwnerId == null || this.ToUserId == null || this.FileId == null)
                throw new Exception("Внутреняя ошибка сервера");
            sh.OwnerId = this.OwnerId;
            sh.ToUserId = this.ToUserId;
            sh.FileId = this.FileId;
            sh.IsWrite = this.IsWrite;
            return sh;
        }
    }
}
