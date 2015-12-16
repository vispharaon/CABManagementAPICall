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
    public class CABHistoryController : ApiController
    {
        private cabmanagementEntities db = new cabmanagementEntities();

        // GET api/CABHistory
        public IEnumerable<tblCABHistory> GettblCABHistories()
        {
            var tblcabhistory = db.tblCABHistory.Include(t => t.tblCAB).Include(t => t.tblCABAnalysis).Include(t => t.tblStatusi);
            return tblcabhistory.AsEnumerable();
        }

        // GET api/CABHistory/5
        public tblCABHistory GettblCABHistory(int id)
        {
            tblCABHistory tblcabhistory = db.tblCABHistory.Find(id);
            if (tblcabhistory == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return tblcabhistory;
        }

        // PUT api/CABHistory/5
        public HttpResponseMessage PuttblCABHistory(int id, tblCABHistory tblcabhistory)
        {
            if (ModelState.IsValid && id == tblcabhistory.CABStatusID)
            {
                db.Entry(tblcabhistory).State = EntityState.Modified;

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

        // POST api/CABHistory
        public HttpResponseMessage PosttblCABHistory(tblCABHistory tblcabhistory)
        {
            if (ModelState.IsValid)
            {
                db.tblCABHistory.Add(tblcabhistory);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, tblcabhistory);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = tblcabhistory.CABStatusID }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/CABHistory/5
        public HttpResponseMessage DeletetblCABHistory(int id)
        {
            tblCABHistory tblcabhistory = db.tblCABHistory.Find(id);
            if (tblcabhistory == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.tblCABHistory.Remove(tblcabhistory);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, tblcabhistory);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}