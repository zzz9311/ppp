namespace DAL.UnitOfWork
{
    public interface IUnitOfWork
    {
        void Save();
        Task SaveAsync(CancellationToken token);
    }
}
