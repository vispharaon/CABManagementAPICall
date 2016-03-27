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
    public class CABVotingController : ApiController
    {
        private cabmanagementEntities3 db = new cabmanagementEntities3();

        //// GET api/CABVoting
        //public IEnumerable<tblCABVoting> GettblCABVotings()
        //{

        //    var tblcabvoting = db.tblCABVoting.Include(t => t.tblDevelopers);
        //    return tblcabvoting.AsEnumerable();
        //}

        public IEnumerable<object> GettblCABs()
        {
            //todo: Dodati join sa tabelom tblStatusi i dodati uslov za tblCABHistory da uzima samo zadnji CABStatusId za CAB_HD_No.
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
                }).Where(x => x.StatusID == 2).Join(db.tblStatusi, all => all.StatusID, ts => ts.StatusID, (all, ts) =>
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

        // GET api/CABVoting/5
        public tblCABVoting GettblCABVoting(string id)
        {
            tblCABVoting tblcabvoting = db.tblCABVoting.Find(id);
            if (tblcabvoting == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return tblcabvoting;
        }

        // PUT api/CABVoting/5
        public HttpResponseMessage PuttblCABVoting(string id, tblCABVoting tblcabvoting)
        {
            if (ModelState.IsValid && id == tblcabvoting.VoterID)
            {
                db.Entry(tblcabvoting).State = EntityState.Modified;

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

        // POST api/CABVoting
        //insert into tblCABVoting and tblCABHistory with status 2 (Where To Go)
        public HttpResponseMessage PosttblCABVoting(int id)
        {
            if (ModelState.IsValid)
            {
                tblCABVoting tblcabvoting = new tblCABVoting();
                tblcabvoting.CAB_HD_No = id;                
                db.tblCABVoting.Add(tblcabvoting);

                tblCABHistory tblcabhistory = new tblCABHistory();
                tblcabhistory.AnalyzeID = db.tblCABHistory.Where(x => x.CAB_HD_No == id).First().AnalyzeID;
                tblcabhistory.CAB_HD_No = id;
                tblcabhistory.StatusID = 2;
                tblcabhistory.StatusDate = DateTime.Now;
                db.tblCABHistory.Add(tblcabhistory);

                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, tblcabvoting);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = tblcabvoting.VoterID }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // POST api/CABVoting
        //insert into tblCABSchedules and tblCABHistory with status 3 (Waiting for Analysis)
        public HttpResponseMessage PosttblCABVoting(int id, string datefrom, string dateto, string voter)
        {
            if (ModelState.IsValid)
            {
                tblCABSchedules tblcabschedules = new tblCABSchedules();
                tblcabschedules.CAB_HD_No = id;
                tblcabschedules.ScheduleDate = DateTime.Now;
                tblcabschedules.ScheduledStart = new DateTime(Convert.ToInt32(datefrom.Split('/')[2]), Convert.ToInt32(datefrom.Split('/')[0]), Convert.ToInt32(datefrom.Split('/')[1]));
                tblcabschedules.ScheduledEnd = new DateTime(Convert.ToInt32(dateto.Split('/')[2]), Convert.ToInt32(dateto.Split('/')[0]), Convert.ToInt32(dateto.Split('/')[1]));
                db.tblCABSchedules.Add(tblcabschedules);

                tblCABHistory tblcabhistory = new tblCABHistory();
                tblcabhistory.AnalyzeID = db.tblCABHistory.Where(x => x.CAB_HD_No == id).First().AnalyzeID;
                tblcabhistory.CAB_HD_No = id;
                tblcabhistory.StatusID = 3;
                tblcabhistory.StatusDate = DateTime.Now;
                db.tblCABHistory.Add(tblcabhistory);

                db.tblCABVoting.Where(x => x.CAB_HD_No == id).Single().CABVoteDate = DateTime.Now;
                db.tblCABVoting.Where(x => x.CAB_HD_No == id).Single().VoterID = voter;
                
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

        // DELETE api/CABVoting/5
        public HttpResponseMessage DeletetblCABVoting(string id)
        {
            tblCABVoting tblcabvoting = db.tblCABVoting.Find(id);
            if (tblcabvoting == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.tblCABVoting.Remove(tblcabvoting);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, tblcabvoting);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}