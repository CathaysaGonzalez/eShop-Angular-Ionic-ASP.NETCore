using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using BE.Dal.EfStructures;
using BE.Dal.Exceptions;
using BE.Models.Entities.Base;

namespace BE.Dal.Repos.Base
{
    public abstract class RepoBase<T> :  IRepo<T> where T: EntityBase,new()
    {
        public DbSet<T> Table { get; }
        public BEIdentityContext Context { get; }
        private readonly bool _disposeContext;

        protected RepoBase(BEIdentityContext context)
        {
            Context = context;
            Table = Context.Set<T>();
        }
        protected RepoBase(DbContextOptions<BEIdentityContext> options) : this(new BEIdentityContext(options))
        {
            _disposeContext = true;
        }
        public virtual void Dispose()
        {
            if (_disposeContext)
            {
                Context.Dispose();
            }
        }

        public T Find(long? id) => Table.Find(id);
        public T FindAsNoTracking(long id) => Table.Where(x => x.Id == id).AsNoTracking().FirstOrDefault();
        public T FindIgnoreQueryFilters(long id) => Table.IgnoreQueryFilters().FirstOrDefault(x => x.Id == id);
        public virtual IEnumerable<T> GetAll() => Table;
        public virtual IEnumerable<T> GetAll(Expression<Func<T, object>> orderBy)
            => Table.OrderBy(orderBy);
        public IEnumerable<T> GetRange(IQueryable<T> query, int skip, int take)
            => query.Skip(skip).Take(take);

        public (string Schema, string TableName) TableSchemaAndName
        {
            get
            {
                return (Context.Model.FindEntityType(typeof(T).FullName).GetSchema(), Context.Model.FindEntityType(typeof(T).FullName).GetTableName());

            }
        }
        public bool HasChanges => Context.ChangeTracker.HasChanges();

        public virtual int Add(T entity, bool persist = true)
        {
            Table.Add(entity);
            return persist ? SaveChanges() : 0;
        }
        public virtual int AddRange(IEnumerable<T> entities, bool persist = true)
        {
            Table.AddRange(entities);
            return persist ? SaveChanges() : 0;
        }
        public virtual int Update(T entity, bool persist = true)
        {
            Table.Update(entity);
            return persist ? SaveChanges() : 0;
        }
        public virtual int UpdateRange(IEnumerable<T> entities, bool persist = true)
        {
            Table.UpdateRange(entities);
            return persist ? SaveChanges() : 0;
        }
        public virtual int Delete(T entity, bool persist = true)
        {
            Table.Remove(entity);
            return persist ? SaveChanges() : 0;
        }
        public virtual int DeleteRange(IEnumerable<T> entities, bool persist = true)
        {
            Table.RemoveRange(entities);
            return persist ? SaveChanges() : 0;
        }
        public int SaveChanges()
        {
            return Context.SaveChanges();
        }
    }
}
