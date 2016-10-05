using System.Configuration;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.OData;
using WebApiNoSQL.DAL;
using WebApiNoSQL.DAL.Domain;
using WebApiNoSQL.DAL.Repositiories;

namespace WebApiNoSQL.Controllers
{    
    [RoutePrefix("api")]
    public class ContactsController : ApiController
    {
        private readonly IContactRepository contacts;

        public ContactsController()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["MongoDbConnectionString"].ConnectionString;
            contacts = new ContactRepository(connectionString);
        }

        /// ?$select=Name
        /// ?$top=2
        /// ?$expand=
        [HttpGet]
        [EnableQuery] 
        [Route("contacts/getAll")]
        public IQueryable<Contact> GetAll()
        {
            return contacts.GetAllContacts().AsQueryable();
        }

        [HttpGet]
        [Route("contacts/getById/{id}")]
        public Contact Get(string id)
        {
            Contact contact = contacts.GetContact(id);
            if (contact == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return contact;
        }

        [HttpPost]
        [Route("contacts/create")]
        public Contact Post(Contact value)
        {
            Contact contact = contacts.AddContact(value);
            return contact;
        }

        [HttpPut]
        [Route("contacts/update")]
        public void Put(string id, Contact value)
        {
            if (!contacts.UpdateContact(id, value))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        [HttpDelete]
        [Route("contacts/delete")]
        public void Delete(string id)
        {
            if (!contacts.RemoveContact(id))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        [HttpGet]
        [Route("contacts/setUpData")]
        public IHttpActionResult SetUpData()
        {
            contacts.ResetCollection();
            return Ok("Operation Successfull");
        }

    }
}