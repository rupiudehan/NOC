using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Noc_App.Context;
using Noc_App.Models.interfaces;
using Noc_App.UtilityService;
using System;
using System.Collections.Generic;
using System.Data;
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
        public async Task<int> CreateWithReturnAsync(T obj)
        {
            try
            {
                await _dbSet.AddAsync(obj);
                
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally { _context.Dispose(); }

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

        public async Task DeleteNonPrimaryAsync(Expression<Func<T, bool>> predicate)
        {
            try
            {
                var entity = await _dbSet.Where(predicate).ToListAsync();
                if (entity.Count()>0)
                {
                    //foreach(var item in entity)
                    //{
                    //    _dbSet.Remove(item);
                    //}
                    _dbSet.RemoveRange(entity);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally { _context.Dispose(); }
            //return 0;
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

        public T GetById(int id)
        {
            try
            {
                return _dbSet.Find(id);
            }
            catch (Exception ex)
            {
                throw;
            }
            finally { _context.Dispose(); }
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
        public async Task<T> GetBylongIdAsync(long id)
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

        public void Update(T entity)
        {
            try
            {
                _context.Entry(entity).State = EntityState.Modified;
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // Log the error or notify the user
                Console.WriteLine("Concurrency exception: " + ex.Message);

                // Reload the entity from the database
                foreach (var entry in ex.Entries)
                {
                    if (entry.State == EntityState.Modified || entry.State == EntityState.Deleted)
                    {
                        // Reload the original values to overwrite the current values
                        entry.Reload();
                    }
                }

                // Retry the operation
                _context.SaveChanges();
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

        public async Task<int> UpdateWithReturnAsync(T entity)
        {
            try
            {
                _context.Entry(entity).State = EntityState.Modified;
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally { _context.Dispose(); }
        }


        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }
        public IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate).ToList();
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

        public IQueryable<T> Include(params string[] navigationProperties)
        {
            IQueryable<T> query = _dbSet;
            foreach (string navigationProperty in navigationProperties)
            {
                query = query.Include(navigationProperty);
            }
            return query;
        }

        public async Task<string> SaveRecords(string name,int unitid,int projecttypeid,string otherprojecttypedetail,string hadbast,string plotno, int villageid
            ,string addressproofphotopath,string kmlfilepath,string kmllinkname,string applicantname,string applicantemailid,string idproofphotopath
            ,string authorizationletterphotopath,int nocpermissiontypeid,int noctypeid,bool isextension,string nocnumber,DateTime previousdate,bool isconfirmed
            ,string applicationid,int processedlevel,bool ispending,DateTime createdon, string khasraItemsJson, string ownerItemsJson)
        {
            try
            {
                string query = $"CALL GrantApplicationCreate('{name}',{unitid},{projecttypeid},'{otherprojecttypedetail ?? ""}','{hadbast}','{plotno}'" +
                    $",{villageid},'{addressproofphotopath}','{kmlfilepath}','{kmllinkname}','{applicantname}','{applicantemailid}','{idproofphotopath}'" +
                    $",'{authorizationletterphotopath}',{nocpermissiontypeid},{noctypeid},{isextension},'{nocnumber ?? ""}','{previousdate}',{isconfirmed}" +
                    $",'{applicationid}',{processedlevel},{ispending},'{createdon}', '{khasraItemsJson}', '{ownerItemsJson}')";
                
                await _context.Database.ExecuteSqlRawAsync(query);
                return "success";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        public async Task<string> ExecuteSaveOrderFunction(string storedProcedureName,/*string[] json, params object[] parameters,*/
            string name, int unitid, int projecttypeid, string otherprojecttypedetail, string hadbast, string plotno, int villageid
            , string addressproofphotopath, string kmlfilepath, string kmllinkname, string applicantname, string applicantemailid, string idproofphotopath
            , string authorizationletterphotopath, int nocpermissiontypeid, int noctypeid, bool isextension, string nocnumber, DateTime previousdate, bool isconfirmed
            , string applicationid, int processedlevel, bool ispending, DateTime createdon, string khasraItemsJson, string ownerItemsJson)
        {
            try
            {
                string query = $"SELECT * FROM {storedProcedureName}('{name}'::text, {unitid}, {projecttypeid}, '{otherprojecttypedetail}'::text, '{hadbast}'::text, '{plotno}'::text, {villageid}, '{addressproofphotopath}'::text, '{ kmlfilepath}'::text,'{kmllinkname}'::text,'{applicantname}'::text,'{applicantemailid}'::text,'{idproofphotopath}'::text,'{authorizationletterphotopath}'::text,{nocpermissiontypeid},{noctypeid},{isextension},'{nocnumber}'::text,'{previousdate}'::timestamp with time zone,{isconfirmed},'{applicationid}'::text,{processedlevel},{ispending},'{createdon}'::timestamp with time zone,'{khasraItemsJson}'::jsonb,'{ownerItemsJson}'::jsonb);";
                //string param = "";
                //for (int i = 1; i <= parameters.Length- json.Length; i++)
                //{
                //    if (i < parameters.Length)
                //    {
                //        if (parameters[i] is string)
                //            param = param == "" ? "@p" + i + " ::text," : param + "@p" + i + " ::text,";
                //        else if (parameters[i] is int)
                //            param = param == "" ? "@p" + i + " ::integer," : param + "@p" + i + " ::integer,";
                //        else if (parameters[i] is bool)
                //            param = param == "" ? "@p" + i + " ::boolean," : param + "@p" + i + " ::boolean,";
                //    }
                //    else {
                //        if (parameters[i] is string)
                //            param = param + "@p" + i+ " ::text"; 
                //        else if (parameters[i] is int)
                //            param = param + "@p" + i + " ::integer";
                //        else if (parameters[i] is bool)
                //            param = param + "@p" + i + " ::boolean";
                //    }
                //}
                //for (int i = 1; i <= json.Length; i++)
                //{
                    
                //    if (i < json.Length)
                //    {
                //        param = param == "" ? ",@p" + Convert.ToInt32(i + parameters.Length - json.Length) + "::jsonb," : param + ",@p" + Convert.ToInt32(i + parameters.Length - json.Length) + "::jsonb,";
                //    }
                //    else param = param + "@p" + Convert.ToInt32(i + parameters.Length - json.Length) + "::jsonb";
                //    parameters[Convert.ToInt32(parameters.Length-json.Length+i-1)] = json[i - 1];
                //}
                
                ////$"@p1, @p1::jsonb";
                //var sql = $"SELECT {storedProcedureName}(" + param + ")";
                ////var parameters = new object[] { customerId, orderItemsJson };

                await _context.Database.ExecuteSqlRawAsync(query);
                return "success";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<List<T>> ExecuteStoredProcedureAsync<T2>(string storedProcedureName, params object[] parameters)
        {
            try
            {
                var parameterNames = string.Join(", ", parameters);
                var sql = $"SELECT * FROM {storedProcedureName} ({parameterNames})";
                List<T> list = await _dbSet.FromSqlRaw(sql).ToListAsync();
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            
        }

        public List<T> ExecuteStoredProcedure<T2>(string storedProcedureName, params object[] parameters)
        {
            try
            {
                var parameterNames = string.Join(", ", parameters);
                var sql = $"SELECT * FROM {storedProcedureName} ({parameterNames})";
                List<T> list = _dbSet.FromSqlRaw(sql).ToList();
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        //public void UpdateUserAssociations(ApplicationUser user, List<int> selectedDivisionIds, List<int> selectedSubdivisionIds, List<int> selectedTehsilIds, List<int> selectedVillageIds)
        //{
        //    // Clear existing associations
        //    user.UserDivisions.Clear();
        //    user.UserSubdivisions.Clear();
        //    user.UserTehsils.Clear();
        //    user.UserVillages.Clear();

        //    // Associate divisions
        //    foreach (var divisionId in selectedDivisionIds)
        //    {
        //        user.UserDivisions.Add(new UserDivision { UserId = user.Id, DivisionId = divisionId });
        //    }

        //    // Associate subdivisions
        //    foreach (var subdivisionId in selectedSubdivisionIds)
        //    {
        //        user.UserSubdivisions.Add(new UserSubdivision { UserId = user.Id, SubdivisionId = subdivisionId });
        //    }

        //    // Associate tehsils
        //    foreach (var tehsilId in selectedTehsilIds)
        //    {
        //        user.UserTehsils.Add(new UserTehsil { UserId = user.Id, TehsilId = tehsilId });
        //    }

        //    // Associate villages
        //    foreach (var villageId in selectedVillageIds)
        //    {
        //        user.UserVillages.Add(new UserVillage { UserId = user.Id, VillageId = villageId });
        //    }

        //    // Save changes to the database
        //    _context.SaveChanges();
        //}
    }
}
