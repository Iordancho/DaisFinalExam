using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DaisFinalExam.Repository.Base;
using DaisFinalExam.Repository.Helpers;
using DaisFinalExam.Repository.Interfaces.Account;
using DaisFinalExam.Repository.Interfaces.User;
using Microsoft.Data.SqlClient;

namespace DaisFinalExam.Repository.Implementations.Account
{
    public class AccountRepository : BaseRepository<Models.Account>, IAccountRepository
    {
        private const string IdDbFieldEnumeratorName = "AccountId";

        protected override string[] GetColumns() => new[]
        {
             "AccountId",
             "AccountName",
             "AccountNumber"
        };

        protected override string GetTableName()
        {
            return "Accounts";
        }

        protected override Models.Account MapEntity(SqlDataReader reader)
        {
            return new Models.Account
            {
                AccountId = Convert.ToInt32(reader["AccountId"]),
                AccountName = Convert.ToString(reader["AccountName"]),
                AccountNumber = Convert.ToString(reader["AccountNumber"])
            };

        }
        public Task<int> CreateAsync(Models.Account entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int objectId)
        {
            throw new NotImplementedException();
        }

        public Task<Models.Account> RetrieveAsync(int objectId)
        {
            return base.RetrieveAsync(IdDbFieldEnumeratorName, objectId);
        }

        public IAsyncEnumerable<Models.Account> RetrieveCollectionAsync(AccountFilter filter)
        {
            Filter commandFilter = new Filter();

            if(filter.AccountNumber is not null)
            {
                commandFilter.AddCondition("AccountNumber", filter.AccountNumber);
            }
            return base.RetrieveCollectionAsync(commandFilter);
        }

        public async Task<bool> UpdateAsync(int objectId, AccountUpdate update)
        {
            using SqlConnection connection = await ConnectionFactory.CreateConnectionAsync();

            var updateCommand = new UpdateCommand(
                connection,
                GetTableName(),
                IdDbFieldEnumeratorName, objectId);

            updateCommand.AddSetClause("Balance", update.Balance);

            return await updateCommand.ExecuteNonQueryAsync() > 0;
        }
    }
}
