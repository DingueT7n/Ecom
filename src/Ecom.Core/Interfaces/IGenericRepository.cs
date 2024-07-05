﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Core.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IReadOnlyList<T>> GetAllAsync();
        IEnumerable<T> GetAll();
        Task<T> GetAsync(T id);

        Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T,bool>>[] includes);
        IEnumerable<T> GetAll(params Expression<Func<T, bool>>[] includes);
        Task<T> GetByIdAsync(T id,params Expression<Func<T, bool>>[] includes);

        Task AddAsync(T entity);
        Task UpdateAsync(T id,T entity);
        Task DeleteAsync(T id);
    }
}
