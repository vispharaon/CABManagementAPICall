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
    public class CABForTaskingController : ApiController
    {
        private cabmanagementEntities3 db = new cabmanagementEntities3();

        // GET api/CABForTasking
        public IEnumerable<object> GettblCABTaskas()
        {
            var maxes = db.tblCABHistory.GroupBy(x => x.CAB_HD_No,     // Key selector
                         x => x.CABStatusID,   // Element selector
                         (key, values) => values.Max()); // Result selector

            var listVotingCABs = db.tblCAB.Join(db.tblCABHistory.Where(x => maxes.Contains(x.CABStatusID)), tc => tc.CAB_HD_No, tch => tch.CAB_HD_No, (tc, tch) =>
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
                }).Where(x => x.StatusID == 8).Join(db.tblStatusi, all => all.StatusID, ts => ts.StatusID, (all, ts) =>
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

        // GET api/CABForTasking/5
        public tblCABTaska GettblCABTaska(int id)
        {
            tblCABTaska tblcabtaska = db.tblCABTaska.Find(id);
            if (tblcabtaska == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return tblcabtaska;
        }

        // PUT api/CABForTasking/5
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

        // POST api/CABForTasking
        public HttpResponseMessage PosttblCABTaska(int id, string username)
        {
            if (ModelState.IsValid)
            {
                tblCABHistory tblcabhistory = new tblCABHistory();
                tblcabhistory.AnalyzeID = db.tblCABHistory.Where(x => x.CAB_HD_No == id).First().AnalyzeID;
                tblcabhistory.CAB_HD_No = id;
                //waiting for tasking
                tblcabhistory.StatusID = 9;
                tblcabhistory.StatusDate = DateTime.Now;
                db.tblCABHistory.Add(tblcabhistory);

                tblCABTaska tblcabtaska = new tblCABTaska();
                tblcabtaska.CAB_HD_No = id;
                tblcabtaska.CAB_Task_ID = 1;
                tblcabtaska.TaskerID = username;
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

        // DELETE api/CABForTasking/5
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