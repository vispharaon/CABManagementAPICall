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
    public class TaskHistoryController : ApiController
    {
        private cabmanagementEntities db = new cabmanagementEntities();

        // GET api/TaskHistory
        public IEnumerable<tblTaskHistory> GettblTaskHistories()
        {
            return db.tblTaskHistory.AsEnumerable();
        }

        // GET api/TaskHistory/5
        public tblTaskHistory GettblTaskHistory(int id)
        {
            tblTaskHistory tbltaskhistory = db.tblTaskHistory.Find(id);
            if (tbltaskhistory == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return tbltaskhistory;
        }

        // PUT api/TaskHistory/5
        public HttpResponseMessage PuttblTaskHistory(int id, tblTaskHistory tbltaskhistory)
        {
            if (ModelState.IsValid && id == tbltaskhistory.TaskHistoryID)
            {
                db.Entry(tbltaskhistory).State = EntityState.Modified;

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

        // POST api/TaskHistory
        public HttpResponseMessage PosttblTaskHistory(tblTaskHistory tbltaskhistory)
        {
            if (ModelState.IsValid)
            {
                db.tblTaskHistory.Add(tbltaskhistory);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, tbltaskhistory);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = tbltaskhistory.TaskHistoryID }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/TaskHistory/5
        public HttpResponseMessage DeletetblTaskHistory(int id)
        {
            tblTaskHistory tbltaskhistory = db.tblTaskHistory.Find(id);
            if (tbltaskhistory == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.tblTaskHistory.Remove(tbltaskhistory);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, tbltaskhistory);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}