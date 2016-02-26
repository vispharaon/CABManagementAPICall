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
    /*public class DevelopersController : ApiController
    {
        private cabmanagementEntities db = new cabmanagementEntities();

        // GET api/Developers
        public IEnumerable<tblDevelopers> GettblDevelopers()
        {
            return db.tblDevelopers.AsEnumerable();
        }

        // GET api/Developers/5
        public tblDevelopers GettblDevelopers(string id)
        {
            tblDevelopers tbldevelopers = db.tblDevelopers.Find(id);
            if (tbldevelopers == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return tbldevelopers;
        }

        // PUT api/Developers/5
        public HttpResponseMessage PuttblDevelopers(string id, tblDevelopers tbldevelopers)
        {
            if (ModelState.IsValid && id == tbldevelopers.DeveloperID)
            {
                db.Entry(tbldevelopers).State = EntityState.Modified;

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

        // POST api/Developers
        public HttpResponseMessage PosttblDevelopers(tblDevelopers tbldevelopers)
        {
            if (ModelState.IsValid)
            {
                db.tblDevelopers.Add(tbldevelopers);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, tbldevelopers);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = tbldevelopers.DeveloperID }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/Developers/5
        public HttpResponseMessage DeletetblDevelopers(string id)
        {
            tblDevelopers tbldevelopers = db.tblDevelopers.Find(id);
            if (tbldevelopers == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.tblDevelopers.Remove(tbldevelopers);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, tbldevelopers);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }*/
}