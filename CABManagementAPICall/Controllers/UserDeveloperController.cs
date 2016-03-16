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
    public class UserDeveloperController : ApiController
    {
        private cabmanagementEntities3 db = new cabmanagementEntities3();
        private cabmanagementEntities3 logDB = new cabmanagementEntities3();

        //// GET api/UserDeveloper
        //public IEnumerable<tblDevelopers> GettblDevelopers()
        //{
        //    return db.tblDevelopers.AsEnumerable();
        //}

        // GET api/UserDeveloper/5
        public tblDevelopers GettblDevelopers(string id)
        {

            var entityLog = new TraceLog();
            entityLog.id = logDB.TraceLog.Max(x => x.id) + 1;
            entityLog.text = id;
            logDB.TraceLog.Add(entityLog);
            logDB.SaveChanges();

            tblDevelopers tbldevelopers = db.tblDevelopers.Where(x => x.DeveloperID == id).FirstOrDefault();
            if (tbldevelopers == null)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Nije pronađen korisnik sa unesenim podacima."));     
            }

            return tbldevelopers;
        }

        // PUT api/UserDeveloper/5
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

        // POST api/UserDeveloper
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

        // DELETE api/UserDeveloper/5
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
    }
}