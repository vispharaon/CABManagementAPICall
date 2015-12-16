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
    public class CABStartEndController : ApiController
    {
        private cabmanagementEntities db = new cabmanagementEntities();

        // GET api/CABStartEnd
        public IEnumerable<tblCABStartEnd> GettblCABStartEnds()
        {
            var tblcabstartend = db.tblCABStartEnd.Include(t => t.tblCAB);
            return tblcabstartend.AsEnumerable();
        }

        // GET api/CABStartEnd/5
        public tblCABStartEnd GettblCABStartEnd(int id)
        {
            tblCABStartEnd tblcabstartend = db.tblCABStartEnd.Find(id);
            if (tblcabstartend == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return tblcabstartend;
        }

        // PUT api/CABStartEnd/5
        public HttpResponseMessage PuttblCABStartEnd(int id, tblCABStartEnd tblcabstartend)
        {
            if (ModelState.IsValid && id == tblcabstartend.CABStartEndID)
            {
                db.Entry(tblcabstartend).State = EntityState.Modified;

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

        // POST api/CABStartEnd
        public HttpResponseMessage PosttblCABStartEnd(tblCABStartEnd tblcabstartend)
        {
            if (ModelState.IsValid)
            {
                db.tblCABStartEnd.Add(tblcabstartend);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, tblcabstartend);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = tblcabstartend.CABStartEndID }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/CABStartEnd/5
        public HttpResponseMessage DeletetblCABStartEnd(int id)
        {
            tblCABStartEnd tblcabstartend = db.tblCABStartEnd.Find(id);
            if (tblcabstartend == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.tblCABStartEnd.Remove(tblcabstartend);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, tblcabstartend);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}