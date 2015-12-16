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
    public class DevelopersTimesController : ApiController
    {
        private cabmanagementEntities db = new cabmanagementEntities();

        // GET api/DevelopersTimes
        public IEnumerable<tblDevelopersTimes> GettblDevelopersTimes()
        {
            return db.tblDevelopersTimes.AsEnumerable();
        }

        // GET api/DevelopersTimes/5
        public tblDevelopersTimes GettblDevelopersTimes(int id)
        {
            tblDevelopersTimes tbldeveloperstimes = db.tblDevelopersTimes.Find(id);
            if (tbldeveloperstimes == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return tbldeveloperstimes;
        }

        // PUT api/DevelopersTimes/5
        public HttpResponseMessage PuttblDevelopersTimes(int id, tblDevelopersTimes tbldeveloperstimes)
        {
            if (ModelState.IsValid && id == tbldeveloperstimes.CAB_Task_ID)
            {
                db.Entry(tbldeveloperstimes).State = EntityState.Modified;

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

        // POST api/DevelopersTimes
        public HttpResponseMessage PosttblDevelopersTimes(tblDevelopersTimes tbldeveloperstimes)
        {
            if (ModelState.IsValid)
            {
                db.tblDevelopersTimes.Add(tbldeveloperstimes);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, tbldeveloperstimes);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = tbldeveloperstimes.CAB_Task_ID }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/DevelopersTimes/5
        public HttpResponseMessage DeletetblDevelopersTimes(int id)
        {
            tblDevelopersTimes tbldeveloperstimes = db.tblDevelopersTimes.Find(id);
            if (tbldeveloperstimes == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.tblDevelopersTimes.Remove(tbldeveloperstimes);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, tbldeveloperstimes);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}