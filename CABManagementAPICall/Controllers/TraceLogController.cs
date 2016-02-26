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
    public class TraceLogController : ApiController
    {
        private cabmanagementEntities3 db = new cabmanagementEntities3();

        // GET api/TraceLog
        public IEnumerable<TraceLog> GetTraceLogs()
        {
            return db.TraceLog.AsEnumerable();
        }

        // GET api/TraceLog/5
        public TraceLog GetTraceLog(int id)
        {
            TraceLog tracelog = db.TraceLog.Find(id);
            if (tracelog == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return tracelog;
        }

        // PUT api/TraceLog/5
        public HttpResponseMessage PutTraceLog(int id, TraceLog tracelog)
        {
            if (ModelState.IsValid && id == tracelog.id)
            {
                db.Entry(tracelog).State = EntityState.Modified;

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

        // POST api/TraceLog
        public HttpResponseMessage PostTraceLog(TraceLog tracelog)
        {
            if (ModelState.IsValid)
            {
                db.TraceLog.Add(tracelog);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, tracelog);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = tracelog.id }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/TraceLog/5
        public HttpResponseMessage DeleteTraceLog(int id)
        {
            TraceLog tracelog = db.TraceLog.Find(id);
            if (tracelog == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.TraceLog.Remove(tracelog);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, tracelog);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}