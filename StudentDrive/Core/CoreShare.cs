namespace Core
{
    using DTO;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    partial class Core
    {
        public List<FileDTO> GetShares(Guid userId)
        {
            var shares = _context.Shares
                .Where(sh => sh.ToUserId == userId)
                .Select(u => u.FileId)
                .ToList();
            if (shares.Count == 0)
                throw new Exception("Связи не найдены");
            var files = _context.Files
                .Where(f => f.IsDeleted == false && shares.Contains(f.Id))
                .ToList();
            return files.Select(u => new FileDTO(u)).ToList();
        }

        public List<ShareDTO> GetShare(Guid FileId)
        {
            var shares = _context.Shares.Where(sh => sh.FileId == FileId).ToList();
            return shares.Select(u => new ShareDTO(u)).ToList();
        }
        public bool GetShareOverride(Guid? FileId, Guid userId)
        {
            return _context.Shares.FirstOrDefault(sh => sh.FileId == FileId && sh.ToUserId == userId).IsWrite;
        }
        public Guid GetShareToUserOwnerUser(Guid? FileId, Guid userId)
        {
            return _context.Shares.FirstOrDefault(sh => sh.FileId == FileId && sh.ToUserId == userId).OwnerId;
        }
        public ShareDTO AddShare(ShareDTO _share)
        {
            var share = _context.Shares.FirstOrDefault(sh => sh.FileId == _share.FileId && sh.ToUserId == _share.ToUserId && sh.OwnerId == _share.OwnerId);
            if (share != null)
                throw new Exception("Данная связь уже существует");
            //var stat = _context.Statistics.First(st => st.UserId == _share.OwnerId);
            //stat.ShareCount++;
            _context.Shares.Add(_share.Map());
            _context.SaveChanges();
            return new ShareDTO(_share.Map());
        }

        public void RemoveAllShare(Guid fileId)
        {
            var shares = _context.Shares.Where(sh => sh.FileId == fileId).ToList();
            _context.Shares.RemoveRange(shares);
            _context.SaveChanges();

        }
        public void DeleteShare(Guid userId, Guid fileId)
        {
            var share = _context.Shares.FirstOrDefault(sh => sh.FileId == fileId && sh.ToUserId == userId);
            if (share == null)
                throw new Exception("Данная связь не существует");
          
            _context.Shares.Remove(share);
            _context.SaveChanges();
        }
        public void DeleteShareByOwner(Guid userId, Guid fileId)
        {
            var share = _context.Shares.FirstOrDefault(sh => sh.FileId == fileId && sh.OwnerId == userId);
            if (share == null)
                throw new Exception("Данная связь не существует");
            _context.Shares.Remove(share);
            _context.SaveChanges();
        }
    }
}
