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
    /*public class CABSchedulesController : ApiController
    {
        private cabmanagementEntities db = new cabmanagementEntities();

        // GET api/CABSchedules
        public IEnumerable<tblCABSchedules> GettblCABSchedules()
        {
            return db.tblCABSchedules.AsEnumerable();
        }

        // GET api/CABSchedules/5
        public tblCABSchedules GettblCABSchedules(int id)
        {
            tblCABSchedules tblcabschedules = db.tblCABSchedules.Find(id);
            if (tblcabschedules == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return tblcabschedules;
        }

        // PUT api/CABSchedules/5
        public HttpResponseMessage PuttblCABSchedules(int id, tblCABSchedules tblcabschedules)
        {
            if (ModelState.IsValid && id == tblcabschedules.ScheduleID)
            {
                db.Entry(tblcabschedules).State = EntityState.Modified;

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

        // POST api/CABSchedules
        public HttpResponseMessage PosttblCABSchedules(tblCABSchedules tblcabschedules)
        {
            if (ModelState.IsValid)
            {
                db.tblCABSchedules.Add(tblcabschedules);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, tblcabschedules);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = tblcabschedules.ScheduleID }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/CABSchedules/5
        public HttpResponseMessage DeletetblCABSchedules(int id)
        {
            tblCABSchedules tblcabschedules = db.tblCABSchedules.Find(id);
            if (tblcabschedules == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.tblCABSchedules.Remove(tblcabschedules);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, tblcabschedules);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }*/
}