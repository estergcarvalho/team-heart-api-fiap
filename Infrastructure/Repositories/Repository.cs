using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TeamHeartFiap.Infrastructure.Data;

namespace TeamHeartFiap.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly AppDbContext _db;
        protected readonly DbSet<T> _set;

        public Repository(AppDbContext db)
        {
            _db = db;
            _set = _db.Set<T>();
        }

        public Task AdicionarAsync(T entidade)
        {
            // versão sem AddAsync
            _set.Add(entidade);
            return Task.CompletedTask;
        }

        public async Task<T?> ObterPorIdAsync(int id)
        {
            // busca manual por ID, já que FindAsync pode não ser desejado generically
            var propId = typeof(T).GetProperty("Id");

            if (propId == null)
                throw new Exception($"A entidade {typeof(T).Name} não possui a propriedade 'Id'.");

            return await _set
                .AsQueryable()
                .FirstOrDefaultAsync(e => (int)propId.GetValue(e)! == id);
        }

        public IQueryable<T> Query()
        {
            return _set.AsQueryable();
        }

        public async Task SalvarAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}