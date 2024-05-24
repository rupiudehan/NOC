using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace Noc_App.Models.interfaces
{
    public interface IRepository<T>
    {
        Task<T> GetByIdAsync(int id);
        Task<T> GetBylongIdAsync(long id);
        IQueryable<T> GetAll();
        Task CreateAsync(T division);
        Task UpdateAsync(T division);
        Task DeleteAsync(int id);
        Task DeleteNonPrimaryAsync(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);
        bool IsDuplicateName(string name);
        bool IsUniqueName(string name,int id);
        IQueryable<T> Include(params string[] navigationProperties);
        Task<List<T>> ExecuteStoredProcedureAsync<T2>(string storedProcedureName, params object[] parameters);
        List<T> ExecuteStoredProcedure<T2>(string storedProcedureName, params object[] parameters);
        //void UpdateUserAssociations(ApplicationUser user, List<int> selectedDivisionIds, List<int> selectedSubdivisionIds, List<int> selectedTehsilIds, List<int> selectedVillageIds);
    }
}
