using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaisFinalExam.Repository.Interfaces.UserAccount
{
    public class UserAccountUpdate
    {
        public SqlDecimal? Balance { get; set; }
        public SqlInt32 AccountId { get; set; }
    }
}
