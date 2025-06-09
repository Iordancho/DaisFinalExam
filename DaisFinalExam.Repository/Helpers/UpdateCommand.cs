using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace DaisFinalExam.Repository.Helpers
{
    public class UpdateCommand : IDisposable
    {
        private List<string> setClauses = new List<string>();
        private SqlCommand sqlCommand;
        private readonly string idDbFieldName;
        private readonly int idDbFieldValue;
        private SqlTransaction transaction;
        private readonly Dictionary<string, object> extraWhereConditions = new();


        public UpdateCommand(
            SqlConnection sqlConnection,
            string tableName,
            string idDbFieldName,
            int idDbFieldValue)
        {
            sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandText = $"UPDATE {tableName}";

            this.idDbFieldName = idDbFieldName;
            this.idDbFieldValue = idDbFieldValue;
        }
        public void AddWhereCondition(string columnName, object value)
        {
            extraWhereConditions[columnName] = value;
        }


        public void AddSetClause(string dbFieldName, object? dbFieldValue)
        {
            if (dbFieldValue is not null)
            {
                setClauses.Add($"[{dbFieldName}] = @{dbFieldName}");
                sqlCommand.Parameters.AddWithValue($"@{dbFieldName}", dbFieldValue);
            }
        }

        public async Task<int> ExecuteNonQueryAsync()
        {
            if (setClauses.Count == 0)
            {
                throw new Exception("No fields to update! You should pass at least one!");
            }

            sqlCommand.CommandText +=
@$" SET {string.Join(", ", setClauses)}
WHERE [{idDbFieldName}] = @{idDbFieldName}";

            sqlCommand.Parameters.AddWithValue($"@{idDbFieldName}", idDbFieldValue);

            foreach (var condition in extraWhereConditions)
            {
                sqlCommand.CommandText += $" AND [{condition.Key}] = @{condition.Key}";
                sqlCommand.Parameters.AddWithValue($"@{condition.Key}", condition.Value);
            }

            transaction = sqlCommand.Connection.BeginTransaction();
            sqlCommand.Transaction = transaction;

            try
            {
                int rowsAffected = await sqlCommand.ExecuteNonQueryAsync();

                if (rowsAffected != 1)
                {
                    transaction.Rollback();
                    throw new Exception($"Just one row should be updated! Command aborted, because {rowsAffected} could have been updated!");
                }

                transaction.Commit();
                return rowsAffected;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public void Dispose()
        {
            transaction?.Dispose();
            sqlCommand?.Dispose();
        }
    }
}