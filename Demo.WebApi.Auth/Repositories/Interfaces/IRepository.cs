using System.Linq;

namespace Demo.WebApi.Auth.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> All();
        void Add(TEntity entity);
    }
}
