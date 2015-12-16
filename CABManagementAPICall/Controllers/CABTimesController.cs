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
    public class CABTimesController : ApiController
    {
        private cabmanagementEntities db = new cabmanagementEntities();

        // GET api/CABTimes
        public IEnumerable<CABTimes> GetCABTimes()
        {
            return db.CABTimes.AsEnumerable();
        }

        // GET api/CABTimes/5
        public CABTimes GetCABTimes(int id)
        {
            CABTimes cabtimes = db.CABTimes.Find(id);
            if (cabtimes == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return cabtimes;
        }

        // PUT api/CABTimes/5
        public HttpResponseMessage PutCABTimes(int id, CABTimes cabtimes)
        {
            if (ModelState.IsValid && id == cabtimes.CAB_HD_No)
            {
                db.Entry(cabtimes).State = EntityState.Modified;

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

        // POST api/CABTimes
        public HttpResponseMessage PostCABTimes(CABTimes cabtimes)
        {
            if (ModelState.IsValid)
            {
                db.CABTimes.Add(cabtimes);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, cabtimes);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = cabtimes.CAB_HD_No }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/CABTimes/5
        public HttpResponseMessage DeleteCABTimes(int id)
        {
            CABTimes cabtimes = db.CABTimes.Find(id);
            if (cabtimes == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.CABTimes.Remove(cabtimes);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, cabtimes);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}