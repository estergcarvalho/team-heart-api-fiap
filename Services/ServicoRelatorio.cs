using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TeamHeartFiap.Data;
using TeamHeartFiap.Domain;

namespace TeamHeartFiap.Services
{
    public class ServicoRelatorio : IServicoRelatorio
    {
        private readonly AppDbContext _db;
        public ServicoRelatorio(AppDbContext db) => _db = db;

        public async Task<IEnumerable<MetricaDiversidade>> GerarRelatorioAsync(DateTime de, DateTime ate)
        {
            var q = _db.MetricasDiversidade.AsNoTracking()
                .Where(m => m.DataRegistro >= de && m.DataRegistro <= ate)
                .OrderByDescending(m => m.DataRegistro);

            return await q.ToListAsync();
        }
    }
}