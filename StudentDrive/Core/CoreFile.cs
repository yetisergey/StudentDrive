namespace Core
{
    using DTO;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    partial class Core
    {
        public List<FileDTO> GetFiles(Guid userId)
        {
            var files = _context.Files.Where(f => f.UserId == userId && f.IsDeleted == false).ToList();
            if (files == null)
                throw new Exception("Внутреняя ошибка сервера. Файлы не найден.");
            var resFile = files.Select(f => new FileDTO(f)).ToList();
            foreach (var obj in resFile)
            {
                if (_context.Shares.FirstOrDefault(sh => sh.FileId == obj.Id) != null)
                {
                    obj.Share = true;
                }
            }
            return resFile;
        }
        public byte[] GetFile(Guid fileId)
        {
            var file = _context.Files.FirstOrDefault(f => f.Id == fileId);
            if (file == null)
                throw new Exception("Внутреняя ошибка сервера. Файл не найден.");
            var stat = _context.Statistics.FirstOrDefault(st => st.UserId == file.UserId);
            stat.DownloadCount++;
            _context.SaveChanges();
            return file.FileSource;
        }
        public List<FileDTO> GetRemoveFiles(Guid userId)
        {
            var files = _context.Files.Where(f => f.UserId == userId && f.IsDeleted == true).ToList();
            if (files == null)
                throw new Exception("Внутреняя ошибка сервера. Файлы не найден.");
            return files.Select(f => new FileDTO(f)).ToList();
        }
        public FileDTO RecoveryFile(Guid fileId)
        {
            var file = _context.Files.FirstOrDefault(f => f.Id == fileId && f.IsDeleted == true);
            if (file == null)
                throw new Exception("Внутреняя ошибка сервера. Файл не найден.");
            file.IsDeleted = false;
            _context.SaveChanges();
            return new FileDTO(file);
        }

        public FileDTO AddFile(FileDTO file)
        {
            file.DateOfUpload = DateTime.Now;
            var nFile = file.Map();
            var ownUser = _context.Users.First(u => u.Id == file.UserId);
            ownUser.DiscUsage = ownUser.DiscUsage + file.FileSource.Length;
            if (ownUser.DiscUsage > ownUser.Space)
                throw new Exception("Недостаточно места в хранилище");
            var stat = _context.Statistics.FirstOrDefault(st => st.UserId == ownUser.Id);
            stat.UploadCount++;
            _context.Files.Add(nFile);
            _context.SaveChanges();
            return new FileDTO(nFile);
        }

        public FileDTO OverWriteFile(Guid oldFileId, FileDTO newFile)
        {
            newFile.DateOfUpload = DateTime.Now;
            var oFile = _context.Files.FirstOrDefault(f => f.Id == oldFileId);
            if (oFile == null)
                throw new Exception("Внутреняя ошибка сервера. Файл не найден.");
            var ownUser = _context.Users.FirstOrDefault(us => us.Id == oFile.UserId);
            ownUser.DiscUsage = ownUser.DiscUsage + newFile.FileSource.Length - oFile.FileSource.Length;
            if (ownUser.DiscUsage > ownUser.Space)
                throw new Exception("Недостаточно места в хранилище");
            var fileShare = _context.Shares.FirstOrDefault(fs => fs.FileId == oFile.Id);
            if (fileShare == null)
                throw new Exception("Внутреняя ошибка сервера. Связь не найдена.");
            if (!fileShare.IsWrite)
                throw new Exception("Внутреняя ошибка сервера. Перезапись невозможна. Нет необходимых прав");
            var nFile = newFile.Map();
            oFile.FileSource = nFile.FileSource;
            oFile.IsDeleted = nFile.IsDeleted;
            oFile.Name = nFile.Name;
            oFile.Size = nFile.Size;
            oFile.UserId = nFile.UserId;
            oFile.DateOfUpload = nFile.DateOfUpload;
            _context.SaveChanges();
            return new FileDTO(oFile);
        }
        public void DeleteFile(Guid fileId)
        {
            var file = _context.Files.FirstOrDefault(f => f.Id == fileId);
            if (file == null)
                throw new Exception("Внутреняя ошибка сервера. Файл не найден.");
            var ownUser = _context.Users.First(u => u.Id == file.UserId);
            var share = _context.Shares.FirstOrDefault(f => f.FileId == fileId && f.OwnerId == ownUser.Id);
            if (share != null)
            {
                _context.Shares.Remove(share);
            }
            if (file.IsDeleted)
            {
                ownUser.DiscUsage = ownUser.DiscUsage - file.FileSource.Length;
                _context.Files.Remove(file);
            }
            else
            {

                file.IsDeleted = true;
            }
            _context.SaveChanges();
        }
    }
}
