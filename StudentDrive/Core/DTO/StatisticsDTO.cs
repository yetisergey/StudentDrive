namespace Core.DTO
{
    using Data;
    public class StatisticsDTO
    {
        public StatisticsDTO() { }
        public StatisticsDTO(Statistics stat)
        {
            UploadCount = stat.UploadCount;
            DownloadCount = stat.DownloadCount;
            ShareCount = stat.ShareCount;
        }
        public int UploadCount { get; }
        public int DownloadCount { get; }
        public int ShareCount { get; }
    }
}