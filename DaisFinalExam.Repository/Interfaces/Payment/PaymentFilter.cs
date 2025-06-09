using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaisFinalExam.Repository.Interfaces.Payment
{
    public class PaymentFilter
    {
        public SqlInt32? FromAccountId { get; set; }
        public SqlString? ToAccountNumber { get; set; }
        public SqlInt32? CreatorId { get; set; }
    }
}
