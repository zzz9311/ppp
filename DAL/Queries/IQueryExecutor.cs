namespace DAL.Queries;
//TODO для интерфейсов отдельная сборка
public interface IQueryExecutor<T>
{
    public Task<T[]> ExecuteAsync(IQueryable<T> queryable, CancellationToken cancellationToken = default);
}