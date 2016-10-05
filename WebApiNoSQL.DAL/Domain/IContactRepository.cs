using System.Collections.Generic;

namespace WebApiNoSQL.DAL.Domain
{
    public interface IContactRepository
    {
        IEnumerable<Contact> GetAllContacts();

        Contact GetContact(string id);

        Contact AddContact(Contact item);

        bool RemoveContact(string id);

        bool UpdateContact(string id, Contact item);

        void ResetCollection();
    }
}