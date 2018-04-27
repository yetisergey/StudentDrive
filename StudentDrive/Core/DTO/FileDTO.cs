namespace Core.DTO
{
    using Data;
    using System;
    public class FileDTO
    {
        public FileDTO() { }
        public FileDTO(File file)
        {
            Id = file.Id;
            UserId = file.UserId;
            Name = file.Name;
            Size = file.Size;
            FileSource = null;
            Share = false;
            DateOfUpload = file.DateOfUpload;
        }
        public Guid? Id { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public float Size { get; set; }
        public byte[] FileSource { get; set; }
        public bool Share { get; set; }
        public bool Rewrite { get; set; }

        public DateTime DateOfUpload { get; set; }


        public File Map(FileDTO fileDto)
        {
            var file = new File();
            file.Id = fileDto.Id ?? Guid.NewGuid();
            file.UserId = fileDto.UserId;
            if (string.IsNullOrEmpty(file.Name))
                throw new Exception("Внутреняя ошибка сервера");
            file.Name = fileDto.Name;
            file.Size = fileDto.Size;
            file.IsDeleted = false;
            file.DateOfUpload = fileDto.DateOfUpload;
            if (fileDto.FileSource != null)
            {
                file.FileSource = fileDto.FileSource;
            }
            else
            {
                throw new Exception("Внутреняя ошибка сервера");
            }

            return file;
        }
        public File Map()
        {
            var file = new File();
            file.Id = this.Id ?? Guid.NewGuid();
            file.UserId = this.UserId;
            if (string.IsNullOrEmpty(this.Name))
                throw new Exception("Внутреняя ошибка сервера");
            file.Name = this.Name;
            file.Size = this.Size;
            file.IsDeleted = false;
            file.DateOfUpload = this.DateOfUpload;
            if (this.FileSource != null)
            {
                file.FileSource = this.FileSource;
            }
            else
            {
                throw new Exception("Внутреняя ошибка сервера");
            }
            return file;
        }
    }
}