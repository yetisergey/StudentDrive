namespace Core
{
    using DTO;
    using System;
    using System.Linq;
    public partial class Core
    {
        public StatisticsDTO GetStatisticsByUserId(Guid UserId)
        {
            var stat = _context.Statistics.FirstOrDefault(st => st.UserId == UserId);
            if (stat == null)
                throw new Exception("Внутреняя ошибка сервера");

            stat.ShareCount = _context.Shares.Count(u => u.OwnerId == UserId);
            _context.SaveChanges();

            return new StatisticsDTO(stat);
        }
    }
}