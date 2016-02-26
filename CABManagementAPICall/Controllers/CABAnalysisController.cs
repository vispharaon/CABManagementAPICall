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
    /*public class CABAnalysisController : ApiController
    {
        private cabmanagementEntities db = new cabmanagementEntities();

        // GET api/CABAnalysis
        public IEnumerable<tblCABAnalysis> GettblCABAnalysis()
        {
            var tblcabanalysis = db.tblCABAnalysis.Include(t => t.tblCAB).Include(t => t.tblDevelopers);
            return tblcabanalysis.AsEnumerable();
        }

        // GET api/CABAnalysis/5
        public tblCABAnalysis GettblCABAnalysis(int id)
        {
            tblCABAnalysis tblcabanalysis = db.tblCABAnalysis.Find(id);
            if (tblcabanalysis == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return tblcabanalysis;
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

        // POST api/CABAnalysis
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
    }*/
}