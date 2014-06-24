using System;
using System.Data.Entity;
using System.Linq;
using DataAccess.DataAccessLayer;

namespace DataAccess.Repository
{
    public interface IRepository<T>
        where T : class
    {
        IQueryable<T> GetAll();
        IQueryable<T> FindBy(System.Linq.Expressions.Expression<Func<T, bool>> predicate);
        T Add(T entity);
        void Delete(T entity);
        void Edit(T entity);
        void Save();
        T Get(System.Linq.Expressions.Expression<Func<T, bool>> predicate);
        void DeleteById(int id);
    }

    public class GenericRepository<T> :
    IRepository<T>
        where T : class
    {
        public GenericRepository(IDataContextManager dataContextManager)
        {
            _entities = dataContextManager.GetDeployHelperContext();
        }

        private DeployHelperContext _entities;
        public DeployHelperContext Context
        {

            get { return _entities; }
            set { _entities = value; }
        }

        public virtual IQueryable<T> GetAll()
        {
            IQueryable<T> query = _entities.Set<T>();
            return query;
        }

        public virtual T Get(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            return _entities.Set<T>().Single(predicate);
        }

        public IQueryable<T> FindBy(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {

            IQueryable<T> query = _entities.Set<T>().Where(predicate);
            return query;
        }

        public virtual T Add(T entity)
        {
            return _entities.Set<T>().Add(entity);
        }

        public virtual void Delete(T entity)
        {
            _entities.Set<T>().Remove(entity);
        }

        public virtual void DeleteById(int id)
        {
            //TODO: this extra query to get the item seems silly when I have the ID
            //Attaching seemed wrong because then I have to create an object which isn't easy of type t.
            var itemToDelete = _entities.Set<T>().Find(id);
            _entities.Set<T>().Remove(itemToDelete);
        }

        public virtual void Edit(T entity)
        {
            _entities.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Save()
        {
            _entities.SaveChanges();
        }

    }
}
