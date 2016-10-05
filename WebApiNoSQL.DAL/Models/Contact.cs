using MongoDB.Bson.Serialization.Attributes;
using System;
using MongoDB.Bson;

namespace WebApiNoSQL.DAL
{
    public class Contact
    {
        [BsonId] // decorates primary key column
        public string Id { get; set; }

        public string Name { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public DateTime LastModified { get; set; }
    }
}