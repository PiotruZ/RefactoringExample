using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegacyApp
{
    public interface IUserService
    {
        bool AddUser(string firstName, string lastName, string email, DateTime dateOfBirth, int clientId);
        bool ApplyCreditLimit(Client client, User user);
        User CreateUser(string firstName, string lastName, string email, DateTime dateOfBirth, object client);
        bool IsUserValidInput(string firstName, string lastName, string email, DateTime dateOfBirth);
    }
}
