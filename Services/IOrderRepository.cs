using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestWebApi.Models;

namespace TestWebApi.Services
{
    public interface IOrderRepository<T> where T : Order
    {
        Task<T> GetById(int Id);
        Task<List<T>> ListAllOrders();
        Task<T> Create(T entity);
        Task<bool> Delete(T entity);
        Task<bool> Update(T entity);
        Task<bool> SaveAll();

    }
}