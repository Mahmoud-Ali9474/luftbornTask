using API.Core;


namespace API.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly LuftbornDBContext _context;
        public UnitOfWork(LuftbornDBContext context)
        {
            _context = context;

        }

        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}