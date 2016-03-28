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
    public class DevelopersController : ApiController
    {
        private cabmanagementEntities3 db = new cabmanagementEntities3();

        // GET api/Developers
        public IEnumerable<tblDevelopers> GettblDevelopers()
        {
            return db.tblDevelopers.AsEnumerable();
        }

        // GET api/Developers/5
        public object GettblDevelopers(int id)
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

        // PUT api/Developers/5
        public HttpResponseMessage PuttblDevelopers(string id, tblDevelopers tbldevelopers)
        {
            if (ModelState.IsValid && id == tbldevelopers.DeveloperID)
            {
                db.Entry(tbldevelopers).State = EntityState.Modified;

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

        // POST api/Developers
        public HttpResponseMessage PosttblDevelopers(tblDevelopers tbldevelopers)
        {
            if (ModelState.IsValid)
            {
                db.tblDevelopers.Add(tbldevelopers);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, tbldevelopers);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = tbldevelopers.DeveloperID }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/Developers/5
        public HttpResponseMessage DeletetblDevelopers(string id)
        {
            tblDevelopers tbldevelopers = db.tblDevelopers.Find(id);
            if (tbldevelopers == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.tblDevelopers.Remove(tbldevelopers);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, tbldevelopers);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}