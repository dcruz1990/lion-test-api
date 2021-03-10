using System;
using System.Collections.Generic;
using System.Threading.Tasks;



namespace TestWebApi.Services
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetById(int Id);
        Task<List<T>> ListAsync();
        Task<T> Create(T entity);
        Task<bool> Delete(T entity);
        Task<bool> Update(T entity);
        Task<bool> SaveAll();

    }
}