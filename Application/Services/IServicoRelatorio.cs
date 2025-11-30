using TeamHeartFiap.Domain;

namespace TeamHeartFiap.Services
{
    public interface IServicoRelatorio
    {
        Task<IEnumerable<MetricaDiversidade>> GerarRelatorioAsync(DateTime de, DateTime ate);
    }
}