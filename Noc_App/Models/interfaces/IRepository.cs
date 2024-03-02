using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace Noc_App.Models.interfaces
{
    public interface IRepository<T>
    {
        Task<T> GetByIdAsync(int id);
        IQueryable<T> GetAll();

        Task CreateAsync(T division);
        Task UpdateAsync(T division);
        Task DeleteAsync(int id);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        bool IsDuplicateName(string name);
        bool IsUniqueName(string name,int id);
    }
}
