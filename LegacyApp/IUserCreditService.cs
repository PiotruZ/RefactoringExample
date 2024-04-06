using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegacyApp
{
    public interface IUserCreditService : IDisposable
    {
        int GetCreditLimit(string lastName, DateTime dateOfBirth);
    }
}
