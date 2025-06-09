using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DaisFinalExam.Repository.Interfaces.User;
using DaisFinalExam.Repository.Base;

namespace DaisFinalExam.Repository.Interfaces.Account
{
    public interface IAccountRepository : IBaseRepository<Models.Account, AccountFilter, AccountUpdate>
    {
    }
}
