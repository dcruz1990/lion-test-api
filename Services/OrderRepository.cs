using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using TestWebApi.Models;
using TestWebApi.Data;

namespace TestWebApi.Services
{

    public class OrderRepository<T> : IOrderRepository<Order>
    {

        private readonly DataContext _dbContext;

        public OrderRepository(DataContext dbContext)
        {
            _dbContext = dbContext;
        }
        public Task<Order> Create(Order entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(Order entity)
        {
            throw new NotImplementedException();
        }

        public Task<Order> GetById(int Id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Order>> ListAllOrders()
        {
            return await _dbContext.Orders.Include(o => o.ProductDetailOnOrder).ToListAsync();
        }

        public Task<bool> SaveAll()
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(Order entity)
        {
            throw new NotImplementedException();
        }
    }
}
