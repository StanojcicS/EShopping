namespace UserManagement.Repositories.Contracts
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAll();
        Task<TEntity> GetById(object id);
        Task<TEntity> Insert(TEntity entity);
        Task<TEntity> Update(TEntity entity, object id);
        Task<TEntity> Delete(TEntity entity);

    }
}
