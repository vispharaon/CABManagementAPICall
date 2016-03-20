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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CABManagementAPICall.Controllers
{
    public class CABController : ApiController
    {
        private cabmanagementEntities3 db = new cabmanagementEntities3();
        private cabmanagementEntities3 logDB = new cabmanagementEntities3();

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
        public object GettblCAB(int id)
        {
            var cabInfo = db.tblCAB.Where(x => x.CAB_HD_No == id).Join(db.tblCABHistory, tc => tc.CAB_HD_No, tch => tch.CAB_HD_No, (tc, tch) =>
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
                }).Join(db.tblStatusi, all => all.StatusID, ts => ts.StatusID, (all, ts) =>
               new
               {
                   CAB_HD_No = all.CAB_HD_No,
                   CAB_Type = all.CAB_Type,
                   CAB_HD_Title = all.CAB_HD_Title,
                   CAB_Sender = all.CAB_Sender,                   
                   CAB_Department = all.CAB_Department,
                   CAB_Priority = all.CAB_Priority,
                   CAB_HD_Date = all.CAB_HD_Date,                   
                   StatusName = ts.StatusDesc,
                   StatusDate = all.StatusDate
               }).FirstOrDefault();

            //tblCAB tblcab = db.tblCAB.Where(x => x.CAB_HD_No == id).FirstOrDefault();
            if (cabInfo == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return cabInfo;
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
        public HttpResponseMessage PosttblCAB(object tblcab)
        {
            if (ModelState.IsValid)
            {

                dynamic d = JObject.Parse(tblcab.ToString());

                tblCAB tblcabobj = MapCabObject(d);

                try
                {
                    db.tblCAB.Add(tblcabobj);

                    tblCABAnalysis tblanalysis = new tblCABAnalysis();
                    tblanalysis.CAB_HD_No = tblcabobj.CAB_HD_No;
                    tblanalysis.tblDevelopers = db.tblDevelopers.FirstOrDefault(x => x.IsAnalyzer == null ? false : x.IsAnalyzer.Value == 1);
                    db.tblCABAnalysis.Add(tblanalysis);

                    tblCABHistory tblhistory = new tblCABHistory();
                    tblhistory.AnalyzeID = tblanalysis.AnalyzeID;
                    tblhistory.CAB_HD_No = tblcabobj.CAB_HD_No;
                    tblhistory.StatusID = db.tblStatusi.Single(x => "Received".Equals(x.StatusDesc)).StatusID;
                    tblhistory.StatusDate = DateTime.Now;
                    db.tblCABHistory.Add(tblhistory);

                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    var entityLog = new TraceLog();
                    entityLog.id = logDB.TraceLog.Max(x => x.id) + 1;
                    entityLog.text = ex.Message;
                    logDB.TraceLog.Add(entityLog);
                    logDB.SaveChanges();
                }               

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, tblcab);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = tblcabobj.CAB_HD_No }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }
        private tblCAB MapCabObject(dynamic tblCABObject)
        {
            //var tblcabobj = (tblCABObject)tblCABObject;            
            tblCAB tblcab = new tblCAB();

            tblcab.CAB_Department = tblCABObject.CAB_Department;
            tblcab.CAB_HD_Date = new DateTime(Convert.ToInt32(tblCABObject.CAB_HD_Date.ToString().Split('.')[2]), Convert.ToInt32(tblCABObject.CAB_HD_Date.ToString().Split('.')[1]), Convert.ToInt32(tblCABObject.CAB_HD_Date.ToString().Split('.')[0]));
            tblcab.CAB_HD_No = Convert.ToInt32(tblCABObject.CAB_HD_No);
            tblcab.CAB_HD_Title = tblCABObject.CAB_HD_Title;
            tblcab.CAB_Note = tblCABObject.CAB_Note;
            tblcab.CAB_Priority = tblCABObject.CAB_Priority;
            tblcab.CAB_Sender = tblCABObject.CAB_Sender;
            tblcab.CAB_Type = tblCABObject.CAB_Type;
            tblcab.Developer_Comment = tblCABObject.Developer_Comment;           

            return tblcab;
        }

        private class tblCABObject
        {
            public string CAB_HD_No;
            public string CAB_Type;
            public string CAB_HD_Date;
            public string CAB_HD_Title;
            public string CAB_Sender;
            public string CAB_Priority;
            public string CAB_Note;
            public string Developer_Comment;
            public string CAB_Department;
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