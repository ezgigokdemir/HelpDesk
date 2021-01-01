using Helpdesk.Data.AppContext;
using Helpdesk.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Helpdesk.Data.Repositories
{
    //Repositoryler arası transaction yönetimi için kullanılır.
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationContext db;
        public UnitOfWork(ApplicationContext context)
        {
            this.db = context;
        }
        public void SaveChanges()
        {
            db.SaveChanges();
        }

        public IRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            return new Repository<TEntity>(db);
        }
    }
}
