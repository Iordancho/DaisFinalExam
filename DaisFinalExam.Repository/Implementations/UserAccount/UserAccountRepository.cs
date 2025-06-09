using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DaisFinalExam.Repository.Base;
using DaisFinalExam.Repository.Helpers;
using DaisFinalExam.Repository.Interfaces.Account;
using DaisFinalExam.Repository.Interfaces.UserAccount;
using Microsoft.Data.SqlClient;

namespace DaisFinalExam.Repository.Implementations.UserAccount
{
    public class UserAccountRepository : BaseRepository<Models.UserAccount>, Interfaces.UserAccount.IUserAccountRepository
    {
        protected override string[] GetColumns() => new[]
        {
            "UserId",
            "AccountId",
            "Balance"
        };


        protected override string GetTableName()
        {
            return "UserAccounts";
        }

        protected override Models.UserAccount MapEntity(SqlDataReader reader)
        {
            return new Models.UserAccount
            {
                UserId = Convert.ToInt32(reader["UserId"]),
                AccountId = Convert.ToInt32(reader["AccountId"]),
                Balance = Convert.ToDecimal(reader["Balance"])
            };

        }
        public Task<int> CreateAsync(Models.UserAccount entity)
        {
            return base.CreateMappingAsync(entity);
        }

        public Task<bool> DeleteAsync(int objectId)
        {
            throw new NotImplementedException();
        }

        public Task<Models.UserAccount> RetrieveAsync(int objectId)
        {
            throw new NotImplementedException();
        }

        public IAsyncEnumerable<Models.UserAccount> RetrieveCollectionAsync(UserAccountFilter filter)
        {
            Filter commandFilter = new Filter();

            if(filter.UserId is not null)
            {
                commandFilter.AddCondition("UserId", filter.UserId);
            }
            if (filter.AccountId is not null)
            {
                commandFilter.AddCondition("AccountId", filter.AccountId);
            }

            return base.RetrieveCollectionAsync(commandFilter);
        }

        public async Task<bool> UpdateAsync(int objectId, UserAccountUpdate update)
        {
            using SqlConnection connection = await ConnectionFactory.CreateConnectionAsync();

            var updateCommand = new UpdateCommand(
                connection,
                GetTableName(),
                "UserId", objectId);

            updateCommand.AddSetClause("Balance", update.Balance);
            updateCommand.AddWhereCondition("AccountId", update.AccountId);

            return await updateCommand.ExecuteNonQueryAsync() > 0;
        }
    }
}
