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
    public class UAFSignedController : ApiController
    {
        private cabmanagementEntities3 db = new cabmanagementEntities3();

        // GET api/UAFSigned
        public IEnumerable<object> GettblCABAnalysis()
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
                }).Where(x => x.StatusID == 28).Join(db.tblStatusi, all => all.StatusID, ts => ts.StatusID, (all, ts) =>
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

        // GET api/UAFSigned/5
        public tblCABAnalysis GettblCABAnalysis(int id)
        {
            tblCABAnalysis tblcabanalysis = db.tblCABAnalysis.Find(id);
            if (tblcabanalysis == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return tblcabanalysis;
        }

        // PUT api/UAFSigned/5
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

        // POST api/UAFSigned
        public HttpResponseMessage PosttblCABAnalysis(tblCABAnalysis tblcabanalysis)
        {
            if (ModelState.IsValid)
            {
                db.tblCABAnalysis.Add(tblcabanalysis);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, tblcabanalysis);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = tblcabanalysis.AnalyzeID }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/UAFSigned/5
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