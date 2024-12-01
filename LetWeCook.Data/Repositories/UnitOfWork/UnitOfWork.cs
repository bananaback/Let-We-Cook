namespace LetWeCook.Data.Repositories.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly LetWeCookDbContext _context;
        public UnitOfWork(LetWeCookDbContext context)
        {
            _context = context;
        }


        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // Save all pending changes to the database and return the number of affected rows
            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
