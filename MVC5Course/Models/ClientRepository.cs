using System;
using System.Linq;
using System.Collections.Generic;

namespace MVC5Course.Models
{
    public class ClientRepository : EFRepository<Client>, IClientRepository
    {
        public Client Find(int id)
        {
            return this.All().FirstOrDefault(c => c.ClientId == id);
        }

        public IQueryable<Client> SearchFirstName(string FirstName)
        {
            var client = this.All();
            if (!string.IsNullOrEmpty(FirstName))
            {
                client = client.Where(c => c.FirstName.Contains(FirstName));
            }
            return client;
        }
    }

    public interface IClientRepository : IRepository<Client>
    {

    }
}