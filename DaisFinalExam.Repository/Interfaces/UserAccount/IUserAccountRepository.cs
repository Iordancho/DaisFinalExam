using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DaisFinalExam.Repository.Interfaces.User;
using DaisFinalExam.Repository.Base;

namespace DaisFinalExam.Repository.Interfaces.UserAccount
{
    public interface IUserAccountRepository : IBaseRepository<Models.UserAccount, UserAccountFilter, UserAccountUpdate>
    {
    }
}
