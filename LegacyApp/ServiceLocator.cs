using LegacyApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegacyApp
{
    public static class ServiceLocator
    {
        private static readonly Dictionary<Type, Func<object>> services = new Dictionary<Type, Func<object>>();

        public static void RegisterService<T>(Func<T> resolver)
        {
            services[typeof(T)] = () => resolver();
        }

        public static T GetService<T>()
        {
            if (services.TryGetValue(typeof(T), out Func<object> resolver))
            {
                return (T)resolver();
            }

            throw new InvalidOperationException($"Service of type {typeof(T).Name} not registered.");
        }

        public static IClientRepository GetClientRepository()
        {
            return GetService<IClientRepository>();
        }

        public static IUserCreditService GetUserCreditService()
        {
            return GetService<IUserCreditService>();
        }
    }

}
