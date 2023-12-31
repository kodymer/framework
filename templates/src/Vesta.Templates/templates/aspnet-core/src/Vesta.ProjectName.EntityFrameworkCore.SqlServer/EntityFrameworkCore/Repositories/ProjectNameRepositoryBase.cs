﻿using Vesta.Ddd.Domain.Entities;
using Vesta.Domain.EntityFrameworkCore.Repositories;
using Vesta.EntityFrameworkCore.Abstracts;

namespace Vesta.ProjectName.EntityFrameworkCore.Repositories
{
    public abstract class ProjectNameRepositoryBase<TEntity, TKey> : EfCoreRepository<ProjectNameDbContext, TEntity, TKey>
        where TEntity : class, IEntity<TKey>
    {
        protected ProjectNameRepositoryBase(IDbContextProvider<ProjectNameDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }
    }

    public abstract class ProjectNameRepositoryBase<TEntity> : ProjectNameRepositoryBase<TEntity, int>
        where TEntity : class, IEntity<int>
    {
        protected ProjectNameRepositoryBase(IDbContextProvider<ProjectNameDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }

}
