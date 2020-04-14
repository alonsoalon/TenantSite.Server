using AlonsoAdmin.Common.Auth;
using FreeSql;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using AlonsoAdmin.MultiTenant.Extensions;
using Microsoft.AspNetCore.Mvc;
using AlonsoAdmin.MultiTenant;
using System;
using System.Linq.Expressions;

namespace AlonsoAdmin.Repository
{
    public abstract class RepositoryBase<TEntity, TKey> : BaseRepository<TEntity, TKey> where TEntity : class, new()
    {
        private readonly IAuthUser _user;
       

        protected RepositoryBase(IFreeSql db, IAuthUser user) : base(db, null, null)
        {
            _user = user;
        }

        public virtual Task<TDto> GetAsync<TDto>(TKey id)
        {
            return Select.WhereDynamic(id).ToOneAsync<TDto>();
        }

        public virtual Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> exp)
        {
            return Select.Where(exp).ToOneAsync();
        }

        public virtual Task<TDto> GetAsync<TDto>(Expression<Func<TEntity, bool>> exp)
        {
            return Select.Where(exp).ToOneAsync<TDto>();
        }

        public async Task<bool> SoftDeleteAsync(TKey id)
        {
            await UpdateDiy
                .SetDto(new { IsDeleted = true, ModifiedUserId = _user.Id, ModifiedUserName = _user.UserName })
                .WhereDynamic(id)
                .ExecuteAffrowsAsync();
            return true;
        }

        public async Task<bool> SoftDeleteAsync(TKey[] ids)
        {
            await UpdateDiy
                .SetDto(new { IsDeleted = true, ModifiedUserId = _user.Id, ModifiedUserName = _user.UserName })
                .WhereDynamic(ids)
                .ExecuteAffrowsAsync();
            return true;
        }
    }

    public abstract class RepositoryBase<TEntity> : RepositoryBase<TEntity, long> where TEntity : class, new()
    {
      
        public RepositoryBase(IFreeSql db, IAuthUser user) : base(db, user)
        { }

    }
}
