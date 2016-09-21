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

        // GET api/TaskHistory/5
        public object GettblTaskHistory(string developerId, bool getTasks)
        {
            var maxes = db.tblCABHistory.GroupBy(x => x.CAB_HD_No,     // Key selector
                         x => x.CABStatusID,   // Element selector
                         (key, values) => values.Max()); // Result selector
            var maxesHistory = db.tblTaskHistory.GroupBy(x => x.CAB_TASK_ID,     // Key selector
                         x => x.TaskHistoryID,   // Element selector
                         (key, values) => values.Max());

            //TraceLog tlog = new TraceLog();
            //tlog.id = db.TraceLog.Max(x => x.id) + 1;
            //tlog.text = maxesHistory.Count().ToString();
            //db.TraceLog.Add(tlog);

            //db.SaveChanges();

            var listTasks = db.tblCAB.Join(db.tblCABHistory.Where(x => maxes.Contains(x.CABStatusID)), tc => tc.CAB_HD_No, tch => tch.CAB_HD_No, (tc, tch) =>
                new
                {
                    CAB_HD_No = tc.CAB_HD_No,
                    CAB_HD_Title = tc.CAB_HD_Title,
                    StatusID = tch.StatusID,
                    StatusDate = tch.StatusDate,

                }).Where(x => x.StatusID > 11 && x.StatusID < 25).Join(db.tblCABTaska, all => all.CAB_HD_No, ts => ts.CAB_HD_No, (all, ts) =>
               new
               {
                   CAB_HD_No = all.CAB_HD_No,
                   CAB_HD_Title = all.CAB_HD_Title,
                   CAB_Task_Text = ts.CAB_Task_Text,
                   CabHD_No_Task = ts.CabHD_No_Task,
                   TaskText = ts.TaskText,
                   WorkingHours = ts.WorkingHours,
                   DeveloperId = ts.DeveloperID,
                   CAB_Task_Id = ts.CAB_Task_ID
               })
               .Where(x => x.DeveloperId == developerId).Join(db.tblTaskHistory.Where(y => maxesHistory.Contains(y.TaskHistoryID)), all => all.CAB_Task_Id, tth => tth.CAB_TASK_ID, (all, tth) =>
               new
               {
                   CAB_HD_No = all.CAB_HD_No,
                   CAB_HD_Title = all.CAB_HD_Title,
                   CAB_Task_Text = all.CAB_Task_Text,
                   CabHD_No_Task = all.CabHD_No_Task,
                   TaskText = all.TaskText,
                   WorkingHours = all.WorkingHours,
                   DeveloperId = all.DeveloperId,
                   CAB_Task_Id = all.CAB_Task_Id,
                   TaskStatusId = tth.StatusID,
                   Percentage = tth.StatusID == 12 || tth.StatusID == 13 ? 0 : tth.StatusID == 14 ? 10 : tth.StatusID == 15 ? 20 : tth.StatusID == 16 ? 30 : tth.StatusID == 17 ? 40 : tth.StatusID == 18 ? 50 : tth.StatusID == 19 ? 60 : tth.StatusID == 20 ? 70 : tth.StatusID == 21 ? 80 : tth.StatusID == 22 ? 90 : 100
               }
               ).Where(x => x.TaskStatusId < 25);

            return listTasks.AsEnumerable();
        }

        //private int GetPercentDoneByStatus(int statusId)
        //{
        //    switch(statusId)
        //    {
        //        case 13: return 0;
        //        case 14: return 10;
        //        case 15: return 20;
        //        case 16: return 30;
        //        case 17: return 40;
        //        case 18: return 50;
        //        case 19: return 60;
        //        case 20: return 70;
        //        case 21: return 80;
        //        case 22: return 90;                    
        //        default: return 100;
        //    }
        //}

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
        public HttpResponseMessage PosttblTaskHistory(int CAB_Task_Id, int Percentage, bool checkedCodeReview)
        {
            if (ModelState.IsValid)
            {
                tblTaskHistory tbltaskhistory = new tblTaskHistory();
                tbltaskhistory.CAB_TASK_ID = CAB_Task_Id;
                tbltaskhistory.DeveloperID = db.tblCABTaska.Single(x => x.CAB_Task_ID == CAB_Task_Id).DeveloperID;
                tbltaskhistory.StatusDate = DateTime.Now;
                tbltaskhistory.StatusID = checkedCodeReview ? 25 : GetStatusForPercentage(Percentage);
                db.tblTaskHistory.Add(tbltaskhistory);

                db.SaveChanges();

                var currentCAB = db.tblCABTaska.Single(x => x.CAB_Task_ID == CAB_Task_Id).CAB_HD_No;
                var newCABStatus = GetNewCABStatus(currentCAB);
                var currentCABStatus = GetCurrentCABStatus(currentCAB);
                if (newCABStatus != currentCABStatus)
                {
                    tblCABHistory tblcabhistory = new tblCABHistory();
                    tblcabhistory.StatusID = newCABStatus;
                    tblcabhistory.StatusDate = DateTime.Now;
                    tblcabhistory.CAB_HD_No = currentCAB;
                    tblcabhistory.AnalyzeID = db.tblCABAnalysis.Single(x => x.CAB_HD_No == currentCAB).AnalyzeID;

                    db.tblCABHistory.Add(tblcabhistory);
                }

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

        private int GetCurrentCABStatus(int cab)
        {
            var maxes = db.tblCABHistory.GroupBy(x => x.CAB_HD_No,     // Key selector
                         x => x.CABStatusID,   // Element selector
                         (key, values) => values.Max()); // Result selector

            return db.tblCABHistory.Single(x => x.CAB_HD_No == cab && maxes.Contains(x.CABStatusID)).StatusID;
        }

        private int GetNewCABStatus(int cab)
        {
            var maxesHistory = db.tblTaskHistory.GroupBy(x => x.CAB_TASK_ID,     // Key selector
                         x => x.TaskHistoryID,   // Element selector
                         (key, values) => values.Max());

            var cabTaskIds = db.tblCABTaska.Where(x => x.CAB_HD_No == cab).Select(x => x.CAB_Task_ID);

            var avgStatus = db.tblTaskHistory.Where(x => cabTaskIds.Contains(x.CAB_TASK_ID) && maxesHistory.Contains(x.TaskHistoryID)).Average(x => x.StatusID);
            return (int) avgStatus;
        }

        private int GetStatusForPercentage(int percentage)
        {
            switch (percentage)
            {
                case 0: return 13;
                case 10: return 14;
                case 20: return 15;
                case 30: return 16;
                case 40: return 17;
                case 50: return 18;
                case 60: return 19;
                case 70: return 20;
                case 80: return 21;
                case 90: return 22;
                case 100: return 24;
                default: return 12;
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