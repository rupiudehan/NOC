
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Noc_App.UtilityService
{

    public static class DbContextExtensions
    {
        public static async Task<List<T>> ExecuteStoredProcedure<T>(
            this DbContext context,
            string storedProcedureName,
            params object[] parameters) where T : class, new()
        {
            var result = new List<T>();

            // Build the command text
            try
            {
                var parameterPlaceholders = string.Join(", ", parameters.Select((p, i) => $"@p{i}"));
                var commandText = $"CALL {storedProcedureName}({parameterPlaceholders})";

                using (var command = context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = commandText;
                    command.CommandType = CommandType.Text;

                    for (int i = 0; i < parameters.Length; i++)
                    {
                        var param = command.CreateParameter();
                        param.ParameterName = $"@p{i}";
                        param.Value = parameters[i];
                        if (parameters[i] is string)
                        {
                            param.DbType = DbType.String;
                        }
                        else if (parameters[i] is int)
                        {
                            param.DbType = DbType.Int32;
                        }
                        command.Parameters.Add(param);
                    }

                    if (command.Connection.State != ConnectionState.Open)
                        command.Connection.Open();
                    var r = await command.ExecuteNonQueryAsync();
                    //using (var reader = await command.ExecuteNonQueryAsync())
                    //{
                    //    var properties = typeof(T).GetProperties();

                    //    while (await reader.ReadAsync())
                    //    {
                    //        var entity = new T();

                    //        foreach (var prop in properties)
                    //        {
                    //            if (!reader.IsDBNull(reader.GetOrdinal(prop.Name)))
                    //            {
                    //                prop.SetValue(entity, reader[prop.Name]);
                    //            }
                    //        }

                    //        result.Add(entity);
                    //    }
                    //}
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return result;
        }
    }

}
