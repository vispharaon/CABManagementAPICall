using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using CABManagementAPICall.Models;

namespace CABManagementAPICall.Controllers
{
    public class UserController : ApiController
    {
        private cabmanagementEntities db = new cabmanagementEntities();

        // GET api/Default1
        public IEnumerable<tblUser> GettblUsers()
        {
            return db.tblUser.AsEnumerable();
        }

        // GET api/Default1/5
        public tblUser GettblUser(int id)
        {
            tblUser tbluser = db.tblUser.Find(id);
            if (tbluser == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return tbluser;
        }

        // GET api/Default1/"username"
        public tblUser GettblUser(string username, string password)
        {
            tblUser tbluser = db.tblUser.Where(x => x.Username == username && x.Password == password).FirstOrDefault();
            if (tbluser == null)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Nije pronađen korisnik sa unesenim podacima."));                
            }

            return tbluser;
        }

        // PUT api/Default1/5
        public HttpResponseMessage PuttblUser(int id, tblUser tbluser)
        {
            if (ModelState.IsValid && id == tbluser.User_ID)
            {
                db.Entry(tbluser).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // POST api/Default1
        public HttpResponseMessage PosttblUser(tblUser tbluser)
        {
            if (ModelState.IsValid)
            {
                db.tblUser.Add(tbluser);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, tbluser);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = tbluser.User_ID }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/Default1/5
        public HttpResponseMessage DeletetblUser(int id)
        {
            tblUser tbluser = db.tblUser.Find(id);
            if (tbluser == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.tblUser.Remove(tbluser);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, tbluser);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}