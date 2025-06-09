using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DaisFinalExam.Repository.Helpers;

namespace DaisFinalExam.Repository.Base
{
    public abstract class BaseRepository<TObj> where TObj : class
    {
        protected abstract string GetTableName();
        protected abstract string[] GetColumns();
        protected virtual string SelectAllCommandText()
        {
            var columns = string.Join(", ", GetColumns());
            return $"SELECT {columns} FROM {GetTableName()} ";
        }
        protected abstract TObj MapEntity(SqlDataReader reader);

        public virtual async Task<int> CreateMappingAsync(TObj entity)
        {
            using SqlConnection connection = await ConnectionFactory.CreateConnectionAsync();
            using SqlCommand command = connection.CreateCommand();

            var properties = typeof(TObj).GetProperties();

            var columns = string.Join(", ", properties.Select(p => p.Name));
            var parameters = string.Join(", ", properties.Select(p => '@' + p.Name));

            command.CommandText = $@"INSERT INTO {GetTableName()} ({columns})
                             VALUES ({parameters});";

            foreach (var prop in properties)
            {
                command.Parameters.AddWithValue('@' + prop.Name, prop.GetValue(entity) ?? DBNull.Value);
            }

            return await command.ExecuteNonQueryAsync(); // returns rows affected, typically 1
        }
        public virtual async Task<int> CreateAsync(TObj entity, string idDbFieldEnumeratorName = null)
        {
            using SqlConnection connection = await ConnectionFactory.CreateConnectionAsync();
            using SqlCommand command = connection.CreateCommand();


            var properties = typeof(TObj).GetProperties()
                .Where(p => p.Name != idDbFieldEnumeratorName);

            var columns = string.Join(", ", properties.Select(p => p.Name));
            var parameters = string.Join(", ", properties.Select(p => '@' + p.Name));

            command.CommandText = $@"INSERT INTO {GetTableName()} ({columns})
                                VALUES ({parameters});
                                SELECT CAST(SCOPE_IDENTITY() as int)";

            foreach(var prop in properties)
            {
                command.Parameters.AddWithValue('@' + prop.Name, prop.GetValue(entity));
            }

            return Convert.ToInt32(await command.ExecuteScalarAsync());

        }
        public virtual async Task<TObj?> RetrieveAsync(string idDbFieldName, int idDbFieldValue)
        {
            using SqlConnection connection = await ConnectionFactory.CreateConnectionAsync();
            using SqlCommand command = connection.CreateCommand();


            command.CommandText = SelectAllCommandText() +
                                $"WHERE {idDbFieldName} = @{idDbFieldName}";

            command.Parameters.AddWithValue($"@{idDbFieldName}", idDbFieldValue);

            using SqlDataReader reader = await command.ExecuteReaderAsync();

            if (reader.Read())
            {
                TObj result = MapEntity(reader);
                if (reader.Read())
                {
                    throw new Exception("Multiple records found for the same ID.");
                }

                return result;
            }
            else
            {
                return null;

            }
        }

        public virtual async IAsyncEnumerable<TObj> RetrieveCollectionAsync(Filter? filter = null)
        {
            using SqlConnection connection = await ConnectionFactory.CreateConnectionAsync();
            using SqlCommand command = connection.CreateCommand();

            command.CommandText = SelectAllCommandText() +
                                $"WHERE 1 = 1";


            foreach (var condition in filter.Conditions)
            {
                command.CommandText += $" AND {condition.Key} = @{condition.Key}";
                command.Parameters.AddWithValue($"@{condition.Key}", condition.Value);
            }

            using SqlDataReader reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                TObj entity = MapEntity(reader);
                yield return entity;
            }

        }
        public virtual async Task<bool> DeleteAsync(string idDbFieldName, int objectId)
        {
            using SqlConnection connection = await ConnectionFactory.CreateConnectionAsync();
            using SqlCommand command = connection.CreateCommand();
            using SqlTransaction transaction = command.Connection.BeginTransaction();

            command.CommandText = $"DELETE FROM {GetTableName()} WHERE {idDbFieldName} = @{idDbFieldName}";
            command.Parameters.AddWithValue($"@{idDbFieldName}", objectId);
            command.Transaction = transaction;

            // Execute the delete command and return the number of affected rows
            int rowsAffected = await command.ExecuteNonQueryAsync();

            if (rowsAffected != 1)
            {
                throw new Exception($"Just one row should be deleted! Command aborted, because {rowsAffected} could have been deleted!");
            }

            transaction.Commit();
            return true;
        }
    }
}
