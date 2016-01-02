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
    public class CABController : ApiController
    {
        private cabmanagementEntities db = new cabmanagementEntities();

        //// GET api/CAB
        //public IEnumerable<tblCAB> GettblCABs()
        //{
        //    return db.tblCAB.AsEnumerable();
        //}

        public IEnumerable<object> GettblCABs()
        {   
            //todo: Dodati join sa tabelom tblStatusi i dodati uslov za tblCABHistory da uzima samo zadnji CABStatusId za CAB_HD_No.
            var listAllCABs = db.tblCAB.Join(db.tblCABHistory, tc => tc.CAB_HD_No, tch => tch.CAB_HD_No, (tc, tch) =>
                new{                    
                    CAB_HD_No = tc.CAB_HD_No,
                    CAB_HD_Title = tc.CAB_HD_Title,
                    CAB_Type = tc.CAB_Type,
                    CAB_HD_Date = tc.CAB_HD_Date,
                    CAB_Sender = tc.CAB_Sender,
                    CAB_Priority = tc.CAB_Priority,
                    CAB_Department = tc.CAB_Department,
                    StatusID = tch.StatusID,
                    StatusDate = tch.StatusDate
           }).Join(db.tblStatusi, all => all.StatusID, ts => ts.StatusID, (all, ts) =>
               new {
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

            return listAllCABs.AsEnumerable();
        }

        // GET api/CAB/5
        public tblCAB GettblCAB(int id)
        {
            tblCAB tblcab = db.tblCAB.Find(id);
            if (tblcab == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return tblcab;
        }

        // PUT api/CAB/5
        public HttpResponseMessage PuttblCAB(int id, tblCAB tblcab)
        {
            if (ModelState.IsValid && id == tblcab.CAB_HD_No)
            {
                db.Entry(tblcab).State = EntityState.Modified;

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

        // POST api/CAB
        public HttpResponseMessage PosttblCAB(tblCAB tblcab)
        {
            if (ModelState.IsValid)
            {
                db.tblCAB.Add(tblcab);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, tblcab);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = tblcab.CAB_HD_No }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/CAB/5
        public HttpResponseMessage DeletetblCAB(int id)
        {
            tblCAB tblcab = db.tblCAB.Find(id);
            if (tblcab == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.tblCAB.Remove(tblcab);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, tblcab);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}