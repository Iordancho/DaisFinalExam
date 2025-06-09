using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DaisFinalExam.Repository.Helpers;
using DaisFinalExam.Repository.Base;
using DaisFinalExam.Repository.Interfaces.User;
using Microsoft.Data.SqlClient;

namespace DaisFinalExam.Repository.Implementations.User
{
    public class UserRepository : BaseRepository<Models.User>, IUserRepository
    {
        private const string IdDbFieldEnumeratorName = "UserId";

        protected override string[] GetColumns() => new[]
        {
            "UserId",
            "Username",
            "Password",
            "FullName"
        };

        protected override string GetTableName()
        {
            return "Users";
        }

        protected override Models.User MapEntity(SqlDataReader reader)
        {
            return new Models.User
            {
                UserId = Convert.ToInt32(reader["UserId"]),
                Username = Convert.ToString(reader["Username"]),
                Password = Convert.ToString(reader["Password"]),
                FullName = Convert.ToString(reader["FullName"])
            };
        }
        public Task<int> CreateAsync(Models.User entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int objectId)
        {
            throw new NotImplementedException();
        }

        public Task<Models.User> RetrieveAsync(int objectId)
        {
            return base.RetrieveAsync(IdDbFieldEnumeratorName, objectId);
        }

        public IAsyncEnumerable<Models.User> RetrieveCollectionAsync(UserFilter filter)
        {

            Filter commandFilter = new Filter();

            if (filter.Username is not null)
            {
                commandFilter.AddCondition("Username", filter.Username);
            }

            return base.RetrieveCollectionAsync(commandFilter);
        }

        public async Task<bool> UpdateAsync(int objectId, UserUpdate update)
        {
            using SqlConnection connection = await ConnectionFactory.CreateConnectionAsync();

            UpdateCommand updateCommand = new UpdateCommand(
                connection,
                "Employees",
                IdDbFieldEnumeratorName, objectId);

            updateCommand.AddSetClause("Password", update.Password);
            updateCommand.AddSetClause("FullName", update.FullName);

            return await updateCommand.ExecuteNonQueryAsync() > 0;
        }
    }
}
