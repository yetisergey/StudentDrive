namespace Core
{
    using System;
    public partial class Core : IDisposable
    {
        private readonly SDContext _context;
        public Core()
        {
            try
            {
                _context = new SDContext();
            }
            catch (Exception e)
            {
                throw new Exception("Внутреняя ошибка сервера");
            }
        }
        public void Dispose()
        {
            _context?.Dispose();
        }        
    }
}
