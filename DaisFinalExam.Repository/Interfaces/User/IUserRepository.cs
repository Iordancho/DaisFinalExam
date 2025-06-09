using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DaisFinalExam.Repository.Base;

namespace DaisFinalExam.Repository.Interfaces.User
{
    public interface IUserRepository : IBaseRepository<Models.User, UserFilter, UserUpdate>
    {
    }
}
