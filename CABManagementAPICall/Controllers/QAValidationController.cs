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
    public class QAValidationController : ApiController
    {
        private cabmanagementEntities3 db = new cabmanagementEntities3();

        // GET api/QAValidation
        public IEnumerable<object> GettblCABTestings()
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
                }).Where(x => x.StatusID == 6 || x.StatusID == 7).Join(db.tblStatusi, all => all.StatusID, ts => ts.StatusID, (all, ts) =>
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

        // GET api/QAValidation/5
        public tblCABTesting GettblCABTesting(int id)
        {
            tblCABTesting tblcabtesting = db.tblCABTesting.Find(id);
            if (tblcabtesting == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return tblcabtesting;
        }

        // PUT api/QAValidation/5
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

        public HttpResponseMessage PosttblCABAnalysis(int id, string qaValidationDescription, string username)
        {
            if (ModelState.IsValid)
            {
                tblCABHistory tblcabhistory = new tblCABHistory();
                tblcabhistory.AnalyzeID = db.tblCABHistory.Where(x => x.CAB_HD_No == id).First().AnalyzeID;
                tblcabhistory.CAB_HD_No = id;
                //waiting for tasking
                tblcabhistory.StatusID = 8;
                tblcabhistory.StatusDate = DateTime.Now;
                db.tblCABHistory.Add(tblcabhistory);

                tblCABTesting tblcabtesting = new tblCABTesting();
                tblcabtesting.CAB_HD_No = id;
                tblcabtesting.Datum_Izmjene = DateTime.Now;
                tblcabtesting.DeveloperID = username;
                tblcabtesting.Status_Test = "2";
                tblcabtesting.Opis = qaValidationDescription;
                db.tblCABTesting.Add(tblcabtesting);

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

        // POST api/QAValidation
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

        // DELETE api/QAValidation/5
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