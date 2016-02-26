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
    /*public class StatusController : ApiController
    {
        private cabmanagementEntities db = new cabmanagementEntities();

        // GET api/Status
        public IEnumerable<tblStatusi> GettblStatusis()
        {
            return db.tblStatusi.AsEnumerable();
        }

        // GET api/Status/5
        public tblStatusi GettblStatusi(int id)
        {
            tblStatusi tblstatusi = db.tblStatusi.Find(id);
            if (tblstatusi == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return tblstatusi;
        }

        // PUT api/Status/5
        public HttpResponseMessage PuttblStatusi(int id, tblStatusi tblstatusi)
        {
            if (ModelState.IsValid && id == tblstatusi.StatusID)
            {
                db.Entry(tblstatusi).State = EntityState.Modified;

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

        // POST api/Status
        public HttpResponseMessage PosttblStatusi(tblStatusi tblstatusi)
        {
            if (ModelState.IsValid)
            {
                db.tblStatusi.Add(tblstatusi);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, tblstatusi);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = tblstatusi.StatusID }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/Status/5
        public HttpResponseMessage DeletetblStatusi(int id)
        {
            tblStatusi tblstatusi = db.tblStatusi.Find(id);
            if (tblstatusi == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.tblStatusi.Remove(tblstatusi);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, tblstatusi);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }*/
}