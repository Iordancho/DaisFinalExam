using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DaisFinalExam.Repository.Base;
using DaisFinalExam.Repository.Helpers;
using DaisFinalExam.Repository.Interfaces.Account;
using DaisFinalExam.Repository.Interfaces.Payment;
using Microsoft.Data.SqlClient;

namespace DaisFinalExam.Repository.Implementations.Payment
{
    public class PaymentRepository : BaseRepository<Models.Payment>, IPaymentRepository
    {
        private const string IdDbFieldEnumeratorName = "PaymentId";

        protected override string[] GetColumns() => new[]
        {
            "PaymentId",
            "CreatorId",
            "FromAccountId",
            "ToAccountNumber",
            "Amount",
            "Reason",
            "Status",
            "CreatedAt"
        };

        protected override string GetTableName()
        {
            return "Payments";
        }

        protected override Models.Payment MapEntity(SqlDataReader reader)
        {
            return new Models.Payment
            {
                PaymentId = Convert.ToInt32(reader["PaymentId"]),
                CreatorId = Convert.ToInt32(reader["CreatorId"]),
                FromAccountId = Convert.ToInt32(reader["FromAccountId"]),
                ToAccountNumber = Convert.ToString(reader["ToAccountNumber"]),
                Amount = Convert.ToDecimal(reader["Amount"]),
                Reason = Convert.ToString(reader["Reason"]),
                Status = Convert.ToString(reader["Status"]),
                CreatedAt = Convert.ToDateTime(reader["CreatedAt"])
            };

        }
        public Task<int> CreateAsync(Models.Payment entity)
        {
            return base.CreateAsync(entity, IdDbFieldEnumeratorName);
        }

        public Task<bool> DeleteAsync(int objectId)
        {
            throw new NotImplementedException();
        }

        public Task<Models.Payment> RetrieveAsync(int objectId)
        {
            return base.RetrieveAsync(IdDbFieldEnumeratorName, objectId);
        }

        public IAsyncEnumerable<Models.Payment> RetrieveCollectionAsync(PaymentFilter filter)
        {
            Filter commandFilter = new Filter();

            if(filter.ToAccountNumber is not null)
            {
                commandFilter.AddCondition("ToAccountNumber", filter.ToAccountNumber);
            }
            if (filter.FromAccountId is not null)
            {
                commandFilter.AddCondition("FromAccountId", filter.FromAccountId);
            }
            if (filter.CreatorId is not null)
            {
                commandFilter.AddCondition("CreatorId", filter.CreatorId);
            }

            return base.RetrieveCollectionAsync(commandFilter);
        }

        public async Task<bool> UpdateAsync(int objectId, PaymentUpdate update)
        {
            using SqlConnection connection = await ConnectionFactory.CreateConnectionAsync();

            var updateCommand = new UpdateCommand(
                connection,
                GetTableName(),
                IdDbFieldEnumeratorName, objectId);

            updateCommand.AddSetClause("Status", update.Status);

            return await updateCommand.ExecuteNonQueryAsync() > 0;
        }
    }
}
