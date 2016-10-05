using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Web.Http.OData;
using System.Linq;
using WebApiNoSQL.DAL.Domain;

namespace WebApiNoSQL.DAL.Repositiories
{
    public class ContactRepository : IContactRepository
    {
        private readonly IMongoClient client;
        private readonly IMongoDatabase database;
        private IMongoCollection<Contact> contacts;

        public ContactRepository(string connection)
        {
            client = new MongoClient(connection);
            database = client.GetDatabase("Contacts");
            contacts = database.GetCollection<Contact>("contacts");
        }

        public IEnumerable<Contact> GetAllContacts()
        {
            var filter = new BsonDocument();
            return contacts.Find(filter).ToEnumerable();
        }

        public Contact GetContact(string id)
        {
            var filter = Builders<Contact>.Filter.Eq("_id", id);
            return contacts.Find(filter).FirstOrDefault();
        }

        public Contact AddContact(Contact item)
        {
            item.Id = ObjectId.GenerateNewId().ToString();
            item.LastModified = DateTime.UtcNow;
            contacts.InsertOne(item);
            return item;
        }

        public bool RemoveContact(string id)
        {
            var filter = Builders<Contact>.Filter.Eq("_id", id);
            var result = contacts.DeleteOne(filter);
            return result.DeletedCount == 1;
        }

        public bool UpdateContact(string id, Contact item)
        {
            var filter = Builders<Contact>.Filter.Eq("_id", id);
            item.LastModified = DateTime.UtcNow;
            var update = Builders<Contact>.Update
                       .Set("Email", item.Email)
                       .Set("Name", item.Name)
                       .Set("Phone", item.Phone)
                       .CurrentDate("LastModified");

            var result = contacts.UpdateOne(filter, update);
            return result.UpsertedId == id;
        }

        public void ResetCollection()
        {
            contacts.DeleteMany(new BsonDocument());

            for (int index = 1; index < 5; index++)
            {
                Contact contact1 = new Contact
                {
                    Email = string.Format("test{0}@example.com", index),
                    Name = string.Format("test{0}", index),
                    Phone = string.Format("{0}{0}{0} {0}{0}{0} {0}{0}{0}{0}", index)
                };
                AddContact(contact1);
            }
        }
    }
}