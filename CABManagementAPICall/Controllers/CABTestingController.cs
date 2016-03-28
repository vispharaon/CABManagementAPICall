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
    public class CABTestingController : ApiController
    {
        private cabmanagementEntities3 db = new cabmanagementEntities3();

        // GET api/CABTesting
        public IEnumerable<tblCABTesting> GettblCABTestings()
        {
            var tblcabtesting = db.tblCABTesting.Include(t => t.tblCAB);
            return tblcabtesting.AsEnumerable();
        }

        // GET api/CABTesting/5
        public object GettblCABTesting(int id)
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

        // PUT api/CABTesting/5
        public HttpResponseMessage PuttblCABTesting(int id, tblCABTesting tblcabtesting)
        {
            if (ModelState.IsValid && id == tblcabtesting.ID)
            {
                db.Entry(tblcabtesting).State = EntityState.Modified;

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

        // POST api/CABTesting
        public HttpResponseMessage PosttblCABTesting(tblCABTesting tblcabtesting)
        {
            if (ModelState.IsValid)
            {
                db.tblCABTesting.Add(tblcabtesting);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, tblcabtesting);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = tblcabtesting.ID }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/CABTesting/5
        public HttpResponseMessage DeletetblCABTesting(int id)
        {
            tblCABTesting tblcabtesting = db.tblCABTesting.Find(id);
            if (tblcabtesting == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.tblCABTesting.Remove(tblcabtesting);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, tblcabtesting);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}