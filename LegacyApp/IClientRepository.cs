using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegacyApp
{
    public interface IClientRepository
    {
        Client GetClientById(int clientId);
    }
}
