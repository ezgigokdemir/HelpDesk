using System;
using System.Collections.Generic;
using System.Text;

namespace Helpdesk.Data.Repositories
{
    public interface IUnitOfWork
    {
        void SaveChanges();

        IRepository<TEntity> Repository<TEntity>() where TEntity : Helpdesk.Model.Entities.BaseEntity;
    }
}
