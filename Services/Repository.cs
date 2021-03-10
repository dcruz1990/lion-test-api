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

    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly DataContext _dbContext;

        public Repository(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<T> Create(T entity)
        {
            _dbContext.Set<T>().Add(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> Delete(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            if (await this.SaveAll())
                return true;

            return false;
        }

        public async Task<T> GetById(int id)
        {
            return await _dbContext.Set<T>().SingleOrDefaultAsync(e => e.Id == id);
        }



        public async Task<List<T>> ListAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task<bool> SaveAll()
        {
            if (await _dbContext.SaveChangesAsync() > 0)
            {
                return true;
            }
            return false;
        }

        // public Task<bool> SaveChangesAsync()
        // {
        //     throw new NotImplementedException();
        // }

        public async Task<bool> Update(T entity)
        {

            _dbContext.Entry(entity).State = EntityState.Modified;

            if (await this.SaveAll())
                return true;

            return false;
        }


    }
}
