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
    public class CABCommentController : ApiController
    {
        private cabmanagementEntities db = new cabmanagementEntities();

        // GET api/CABComment
        public IEnumerable<tblCABComment> GettblCABComments()
        {
            var tblcabcomment = db.tblCABComment.Include(t => t.tblCAB).Include(t => t.tblDevelopers).Include(t => t.tblStatusi);
            return tblcabcomment.AsEnumerable();
        }

        // GET api/CABComment/5
        public tblCABComment GettblCABComment(int id)
        {
            tblCABComment tblcabcomment = db.tblCABComment.Find(id);
            if (tblcabcomment == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return tblcabcomment;
        }

        // PUT api/CABComment/5
        public HttpResponseMessage PuttblCABComment(int id, tblCABComment tblcabcomment)
        {
            if (ModelState.IsValid && id == tblcabcomment.ID)
            {
                db.Entry(tblcabcomment).State = EntityState.Modified;

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

        // POST api/CABComment
        public HttpResponseMessage PosttblCABComment(tblCABComment tblcabcomment)
        {
            if (ModelState.IsValid)
            {
                db.tblCABComment.Add(tblcabcomment);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, tblcabcomment);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = tblcabcomment.ID }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/CABComment/5
        public HttpResponseMessage DeletetblCABComment(int id)
        {
            tblCABComment tblcabcomment = db.tblCABComment.Find(id);
            if (tblcabcomment == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.tblCABComment.Remove(tblcabcomment);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, tblcabcomment);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}