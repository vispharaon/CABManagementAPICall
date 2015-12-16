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
        private cabmanagementEntities db = new cabmanagementEntities();

        // GET api/CABVoting
        public IEnumerable<tblCABVoting> GettblCABVotings()
        {
            var tblcabvoting = db.tblCABVoting.Include(t => t.tblDevelopers);
            return tblcabvoting.AsEnumerable();
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
        public HttpResponseMessage PosttblCABVoting(tblCABVoting tblcabvoting)
        {
            if (ModelState.IsValid)
            {
                db.tblCABVoting.Add(tblcabvoting);
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