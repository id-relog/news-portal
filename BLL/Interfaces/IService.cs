namespace BLL.Interfaces
{
    public interface IService<T> where T : class
    {
        bool Add(T entity);
        bool Delete(Guid entityId);
        bool Edit(T entity);

        T? Get(Guid entityId);
        List<T> Get();
    }
}