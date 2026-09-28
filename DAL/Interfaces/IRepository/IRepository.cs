namespace DAL.Interfaces
{
    public interface IRepository<T> where T : class
    {
        bool Add(T entity);
        bool Edit(T entity);
        bool Delete(Guid entityId);

        T? Get(Guid entityId);
        List<T> Get(); 
    }
}