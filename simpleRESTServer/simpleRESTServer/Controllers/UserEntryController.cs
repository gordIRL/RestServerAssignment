using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;



namespace simpleRESTServer.Controllers
{
    public class UserEntryController : ApiController
    {
        // GET: api/UserEntry
        public List<UserEntry> Get()
        {
            // use a paging mechanism here if there is a large number of records in the database!!
            UserEntryPersistence uep = new UserEntryPersistence();
            return uep.GetUserEntries();            
        }



        // GET: api/UserEntry/5
        public UserEntry Get(int id)
        {
            UserEntryPersistence uep = new UserEntryPersistence();
            UserEntry userEntry = uep.GetUserEntry(id);
            return userEntry;
        }




        // POST: api/UserEntry
        public HttpResponseMessage Post([FromBody]UserEntry value)
        {
            UserEntryPersistence uep = new UserEntryPersistence();
            int id = uep.SaveUserEntry(value);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);
                       
            // response coming back should include the location of the new resource
            response.Headers.Location = new Uri(Request.RequestUri, string.Format("UserEntry/{0}", id));
            
            return response;
        }



        // PUT: api/UserEntry/5
        public HttpResponseMessage Put(int id, [FromBody]UserEntry value)
        {
            UserEntryPersistence uep = new UserEntryPersistence();
            bool recordExisted = false;
            recordExisted = uep.UpdateUserEntry(id, value);

            HttpResponseMessage response;

            if (recordExisted)
            {
                // 204
                response = Request.CreateResponse(HttpStatusCode.NoContent);
            }
            else
            {
                // 404
                response = Request.CreateResponse(HttpStatusCode.NotFound);
            }
            return response;
        }



        // DELETE: api/UserEntry/5
        public HttpResponseMessage Delete(int id)
        {
            UserEntryPersistence uep = new UserEntryPersistence();
            bool recordExisted = false;

            recordExisted = uep.DeleteUserEntry(id);

            HttpResponseMessage response;

            if (recordExisted)
            {
                // 204
                response = Request.CreateResponse(HttpStatusCode.NoContent);
            }
            else
            {
                // 404
                response = Request.CreateResponse(HttpStatusCode.NotFound);
            }
            return response;
        }
    }
}
