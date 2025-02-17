using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Noc_App.Models.interfaces
{
    public interface IRepository<T>
    {
        Task<T> GetByIdAsync(int id);
        T GetById(int id);
        Task<T> GetBylongIdAsync(long id);
        IQueryable<T> GetAll();
        Task<int> CreateWithReturnAsync(T obj);
        Task<int> UpdateWithReturnAsync(T entity);
        Task CreateAsync(T entity);
        Task UpdateAsync(T entity);
        void Update(T entity);
        Task DeleteAsync(int id);
        Task DeleteNonPrimaryAsync(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);
        bool IsDuplicateName(string name);
        bool IsUniqueName(string name,int id);
        IQueryable<T> Include(params string[] navigationProperties);
        Task<List<T>> ExecuteStoredProcedureAsync<T2>(string storedProcedureName, params object[] parameters);
        List<T> ExecuteStoredProcedure<T2>(string storedProcedureName, params object[] parameters);
        //Task<string> ExecuteSaveOrderFunction(string storedProcedureName, string[] json, params object[] parameters);
        Task<string> SaveRecords(string name, int unitid, int projecttypeid, string otherprojecttypedetail, string hadbast, string plotno, int villageid
            , string addressproofphotopath, string kmlfilepath, string kmllinkname, string applicantname, string applicantemailid, string idproofphotopath
            , string authorizationletterphotopath, int nocpermissiontypeid, int noctypeid, bool isextension, string nocnumber, DateTime previousdate, bool isconfirmed
            , string applicationid, int processedlevel, bool ispending, DateTime createdon, string khasraItemsJson, string ownerItemsJson);
        Task<string> ExecuteSaveOrderFunction(string storedProcedureName,/*string[] json, params object[] parameters,*/
            string name, int unitid, int projecttypeid, string otherprojecttypedetail, string hadbast, string plotno, int villageid
            , string addressproofphotopath, string kmlfilepath, string kmllinkname, string applicantname, string applicantemailid, string idproofphotopath
            , string authorizationletterphotopath, int nocpermissiontypeid, int noctypeid, bool isextension, string nocnumber, DateTime previousdate, bool isconfirmed
            , string applicationid, int processedlevel, bool ispending, DateTime createdon, string khasraItemsJson, string ownerItemsJson);
        //void UpdateUserAssociations(ApplicationUser user, List<int> selectedDivisionIds, List<int> selectedSubdivisionIds, List<int> selectedTehsilIds, List<int> selectedVillageIds);
    }
}
