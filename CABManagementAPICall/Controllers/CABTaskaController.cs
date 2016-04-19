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
    public class CABTaskaController : ApiController
    {
        private cabmanagementEntities3 db = new cabmanagementEntities3();

        // GET api/CABTaska
        public IEnumerable<object> GettblCABTaskas()
        {
            var maxes = db.tblCABHistory.GroupBy(x => x.CAB_HD_No,     // Key selector
                         x => x.CABStatusID,   // Element selector
                         (key, values) => values.Max()); // Result selector

            var listTaskingCABs = db.tblCAB.Join(db.tblCABHistory.Where(x => maxes.Contains(x.CABStatusID)), tc => tc.CAB_HD_No, tch => tch.CAB_HD_No, (tc, tch) =>
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
                }).Where(x => x.StatusID == 9).Join(db.tblStatusi, all => all.StatusID, ts => ts.StatusID, (all, ts) =>
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

            return listTaskingCABs.AsEnumerable();
        }

        // GET api/CABTaska/5
        public object GettblCABTaska(int id)
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

        // GET api/CABTaska/5
        public IEnumerable<object> GettblCABTaska(int id, bool isTasking)
        {
            var allTasks = db.tblCABTaska.Where(x => x.CAB_HD_No == id).Join(db.tblCAB, tv => tv.CAB_HD_No, tcs => tcs.CAB_HD_No, (tv, tcs) =>
                new
                {
                    CAB_Task_Id = tv.CAB_Task_ID,
                    CabHD_No_Task = tv.CabHD_No_Task == null ? 1 : tv.CabHD_No_Task,
                    CAB_Task_Text = tv.CAB_Task_Text == null ? "" : tv.CAB_Task_Text,
                    WorkingHours = tv.WorkingHours == null ? 0 : tv.WorkingHours,
                    CAB_Task_Create_Date = tv.CAB_Task_Create_Date,
                    Task_Text = tv.TaskText
                });

            return allTasks;
        }

        // PUT api/CABTaska/5
        public HttpResponseMessage PuttblCABTaska(int id, tblCABTaska tblcabtaska)
        {
            if (ModelState.IsValid && id == tblcabtaska.CAB_Task_ID)
            {
                db.Entry(tblcabtaska).State = EntityState.Modified;

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

        // POST api/CABTaska
        public HttpResponseMessage PosttblCABTaska(int CAB_Task_Id, int CabHD_No_Task, string CAB_Task_Text, int WorkingHours, string Task_Text)
        {
            if (ModelState.IsValid)
            {
                db.tblCABTaska.Single(x => x.CAB_Task_ID == CAB_Task_Id).CabHD_No_Task = CabHD_No_Task;
                db.tblCABTaska.Single(x => x.CAB_Task_ID == CAB_Task_Id).CAB_Task_Text = CAB_Task_Text;
                db.tblCABTaska.Single(x => x.CAB_Task_ID == CAB_Task_Id).WorkingHours = WorkingHours;
                db.tblCABTaska.Single(x => x.CAB_Task_ID == CAB_Task_Id).TaskText = Task_Text;
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, new tblCABTaska());
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { CAB_Task_Id }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // POST api/CABTaska
        public HttpResponseMessage PosttblCABTaska(int id, int CabHD_No_Task, string CAB_Task_Text, int WorkingHours, string Task_Text, string username)
        {
            if (ModelState.IsValid)
            {
                tblCABTaska tblcabtaska = new tblCABTaska();
                tblcabtaska.CAB_HD_No = id;
                tblcabtaska.CAB_Task_Create_Date = DateTime.Now;
                tblcabtaska.CAB_Task_Text = CAB_Task_Text;
                tblcabtaska.CabHD_No_Task = CabHD_No_Task;
                tblcabtaska.TaskerID = username;
                tblcabtaska.TaskText = Task_Text;

                db.tblCABTaska.Add(tblcabtaska);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, tblcabtaska);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = tblcabtaska.CAB_Task_ID }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // POST api/CABTaska
        public HttpResponseMessage PosttblCABTaska(string developerId, int id)
        {
            if (ModelState.IsValid)
            {
                db.tblCABTaska.Where(x => x.CAB_HD_No == id).ToList().ForEach(x =>
                {
                    x.DeveloperID = developerId;

                    tblTaskHistory tbltaskhistory = new tblTaskHistory();
                    tbltaskhistory.DeveloperID = developerId;
                    tbltaskhistory.StatusDate = DateTime.Now;
                    tbltaskhistory.StatusID = 12;
                    tbltaskhistory.CAB_TASK_ID = x.CAB_Task_ID;

                    db.tblTaskHistory.Add(tbltaskhistory);
                });

                tblCABHistory tblcabhistory = new tblCABHistory();
                tblcabhistory.AnalyzeID = db.tblCABHistory.Where(x => x.CAB_HD_No == id).First().AnalyzeID;
                tblcabhistory.CAB_HD_No = id;
                tblcabhistory.StatusID = 12;
                tblcabhistory.StatusDate = DateTime.Now;
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


        // DELETE api/CABTaska/5
        public HttpResponseMessage DeletetblCABTaska(int id)
        {
            tblCABTaska tblcabtaska = db.tblCABTaska.Find(id);
            if (tblcabtaska == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.tblCABTaska.Remove(tblcabtaska);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, tblcabtaska);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}