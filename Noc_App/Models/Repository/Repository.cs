using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Noc_App.Context;
using Noc_App.Models.interfaces;
using System.Linq.Expressions;

namespace Noc_App.Models.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<T> _dbSet;
        public Repository(ApplicationDbContext context)
        {
            //_context = context;
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = _context.Set<T>();
        }
        public async Task CreateAsync(T obj)
        {
            try
            {
                await _dbSet.AddAsync(obj);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally { _context.Dispose(); }

        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var entity = await GetByIdAsync(id);
                if (entity != null)
                {
                    _dbSet.Remove(entity);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally { _context.Dispose(); }

        }

        public IQueryable<T> GetAll()
        {
            try
            {
                return _dbSet.AsQueryable();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally { _context.Dispose(); }
            //return null;
        }

        public async Task<T> GetByIdAsync(int id)
        {
            try
            {
                return await _dbSet.FindAsync(id);
            }
            catch (Exception ex)
            {
                throw;
            }
            finally { _context.Dispose(); }
        }

        public async Task UpdateAsync(T entity)
        {
            try
            {
                _context.Entry(entity).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally { _context.Dispose(); }
        }


        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }
        public bool IsDuplicateName(string name)
        {
            var result = _dbSet.AsEnumerable().Any(e => e.GetType().GetProperty("Name").GetValue(e).ToString() == name);
            return result;
        }

        public bool IsUniqueName(string name, int id)
        {
            return _dbSet.AsEnumerable().Any(e => e.GetType().GetProperty("Name").GetValue(e).ToString() == name && e.GetType().GetProperty("Id").GetValue(e).ToString()!=id.ToString());
        }
    }
}
