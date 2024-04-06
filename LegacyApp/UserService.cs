using System;

namespace LegacyApp
{
    public class UserService : IUserService
    {
        private readonly IClientRepository _clientRepository;
        private readonly IUserCreditService _userCreditService;

        public UserService()
        {
            ServiceLocator.RegisterService<IClientRepository>(() => new ClientRepository());
            ServiceLocator.RegisterService<IUserCreditService>(() => new UserCreditService());
            try
            {
                _clientRepository = ServiceLocator.GetClientRepository();
                _userCreditService = ServiceLocator.GetUserCreditService();
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine(e.Message);
            }
        }


        public bool AddUser(string firstName, string lastName, string email, DateTime dateOfBirth, int clientId)
        {
            if (!IsUserValidInput(firstName, lastName, email, dateOfBirth)) return false;

            var client = _clientRepository.GetClientById(clientId);

            var user = CreateUser(firstName, lastName, email, dateOfBirth, client);

            if (!ApplyCreditLimit(client, user)) return false;

            UserDataAccess.AddUser(user);
            return true;
        }

        public bool ApplyCreditLimit(Client client, User user)
        {
            if (client.Type == "VeryImportantClient")
            {
                user.HasCreditLimit = false;
            }
            else if (client.Type == "ImportantClient")
            {
                using (var userCreditService = new UserCreditService())
                {
                    int creditLimit = userCreditService.GetCreditLimit(user.LastName, user.DateOfBirth);
                    creditLimit = creditLimit * 2;
                    user.CreditLimit = creditLimit;
                }
            }
            else
            {
                user.HasCreditLimit = true;
                using (var userCreditService = new UserCreditService())
                {
                    int creditLimit = userCreditService.GetCreditLimit(user.LastName, user.DateOfBirth);
                    user.CreditLimit = creditLimit;
                }
            }

            if (user.HasCreditLimit && user.CreditLimit < 500)
            {
                return false;
            }
            return true;
        }

        public User CreateUser(string firstName, string lastName, string email, DateTime dateOfBirth, object client)
        {
            return new User
            {
                Client = client,
                DateOfBirth = dateOfBirth,
                EmailAddress = email,
                FirstName = firstName,
                LastName = lastName
            };
        }

        public bool IsUserValidInput(string firstName, string lastName, string email, DateTime dateOfBirth)
        {
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
            {
                return false;
            }

            if (!email.Contains("@") && !email.Contains("."))
            {
                return false;
            }

            var now = DateTime.Now;
            int age = now.Year - dateOfBirth.Year;
            if (now.Month < dateOfBirth.Month || (now.Month == dateOfBirth.Month && now.Day < dateOfBirth.Day)) age--;

            if (age < 21)
            {
                return false;
            }
            return true;
        }
    }
}

