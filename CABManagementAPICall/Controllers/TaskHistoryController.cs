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
        private cabmanagementEntities3 db = new cabmanagementEntities3();

        // GET api/TaskHistory
        public IEnumerable<tblTaskHistory> GettblTaskHistories()
        {
            return db.tblTaskHistory.AsEnumerable();
        }

        // GET api/TaskHistory/5
        public object GettblTaskHistory(int id)
        {
            var votingCAB = db.tblCABVoting.Join(db.tblCABSchedules, tv => tv.CAB_HD_No, tcs => tcs.CAB_HD_No, (tv, tcs) =>
                new
                {
                    CAB_HD_No = tv.CAB_HD_No,
                    devId = tv.VoterID,
                    votedate = tv.CABVoteDate,
                    votedatefrom = tcs.ScheduledStart,
                    votedateto = tcs.ScheduledEnd,
                }).Where(x => x.CAB_HD_No == id).Join(db.tblDevelopers, all => all.devId, td => td.DeveloperID, (all, td) =>
               new
               {
                   CAB_HD_No = all.CAB_HD_No,
                   votename = td.Ime,
                   votelastname = td.Prezime,
                   votedate = all.votedate,
                   votedatefrom = all.votedatefrom,
                   votedateto = all.votedateto,
               }).SingleOrDefault();

            return votingCAB;
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