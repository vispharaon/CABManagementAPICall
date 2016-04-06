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
    public class CABAnalysisController : ApiController
    {
        private cabmanagementEntities3 db = new cabmanagementEntities3();

        // GET api/CABAnalysis
        public IEnumerable<object> GettblCABAnalysis()
        {
            var listVotingCABs = db.tblCAB.Join(db.tblCABHistory, tc => tc.CAB_HD_No, tch => tch.CAB_HD_No, (tc, tch) =>
                new
                {
                    CAB_HD_No = tc.CAB_HD_No,
                    CAB_HD_Title = tc.CAB_HD_Title,
                    CAB_Type = tc.CAB_Type,
                    CAB_HD_Date = tc.CAB_HD_Date,
                    CAB_Sender = tc.CAB_Sender,
                    CAB_Priority = tc.CAB_Priority,
                    CAB_Department = tc.CAB_Department,
                    StatusID = tch.StatusID,
                    StatusDate = tch.StatusDate
                }).Where(x => x.StatusID == 3 || x.StatusID == 5).Join(db.tblStatusi, all => all.StatusID, ts => ts.StatusID, (all, ts) =>
               new
               {
                   CAB_HD_No = all.CAB_HD_No,
                   CAB_HD_Title = all.CAB_HD_Title,
                   CAB_Type = all.CAB_Type,
                   CAB_HD_Date = all.CAB_HD_Date,
                   CAB_Sender = all.CAB_Sender,
                   CAB_Priority = all.CAB_Priority,
                   CAB_Department = all.CAB_Department,
                   StatusName = ts.StatusDesc,
                   StatusDate = all.StatusDate
               });

            return listVotingCABs.AsEnumerable();
        }

        // GET api/CABAnalysis/5
        public object GettblCABAnalysis(int id)
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

        // PUT api/CABAnalysis/5
        public HttpResponseMessage PuttblCABAnalysis(int id, tblCABAnalysis tblcabanalysis)
        {
            if (ModelState.IsValid && id == tblcabanalysis.AnalyzeID)
            {
                db.Entry(tblcabanalysis).State = EntityState.Modified;

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

        //TODO: napraviti PosttblCABAbalysis sa ostalim parametrima.

        // POST api/CABAnalysis
        public HttpResponseMessage PosttblCABAnalysis(int id, int status)
        {
            if (ModelState.IsValid)
            {               

                tblCABHistory tblcabhistory = new tblCABHistory();
                tblcabhistory.AnalyzeID = db.tblCABHistory.Where(x => x.CAB_HD_No == id).First().AnalyzeID;
                tblcabhistory.CAB_HD_No = id;
                tblcabhistory.StatusID = status;
                tblcabhistory.StatusDate = DateTime.Now;
                db.tblCABHistory.Add(tblcabhistory);

                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, tblcabhistory);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = tblcabhistory.AnalyzeID }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/CABAnalysis/5
        public HttpResponseMessage DeletetblCABAnalysis(int id)
        {
            tblCABAnalysis tblcabanalysis = db.tblCABAnalysis.Find(id);
            if (tblcabanalysis == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.tblCABAnalysis.Remove(tblcabanalysis);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, tblcabanalysis);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}