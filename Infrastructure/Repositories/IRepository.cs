namespace TeamHeartFiap.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task AdicionarAsync(T entidade);
        Task<T?> ObterPorIdAsync(int id);
        IQueryable<T> Query();
        Task SalvarAsync();
    }
}