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
    public class AttachmentController : ApiController
    {
        private cabmanagementEntities db = new cabmanagementEntities();

        // GET api/Attachment
        public IEnumerable<tblAttachments> GettblAttachments()
        {
            var tblattachments = db.tblAttachments.Include(t => t.tblCAB);
            return tblattachments.AsEnumerable();
        }

        // GET api/Attachment/5
        public tblAttachments GettblAttachments(int id)
        {
            tblAttachments tblattachments = db.tblAttachments.Find(id);
            if (tblattachments == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return tblattachments;
        }

        // PUT api/Attachment/5
        public HttpResponseMessage PuttblAttachments(int id, tblAttachments tblattachments)
        {
            if (ModelState.IsValid && id == tblattachments.AttachID)
            {
                db.Entry(tblattachments).State = EntityState.Modified;

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

        // POST api/Attachment
        public HttpResponseMessage PosttblAttachments(tblAttachments tblattachments)
        {
            if (ModelState.IsValid)
            {
                db.tblAttachments.Add(tblattachments);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, tblattachments);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = tblattachments.AttachID }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/Attachment/5
        public HttpResponseMessage DeletetblAttachments(int id)
        {
            tblAttachments tblattachments = db.tblAttachments.Find(id);
            if (tblattachments == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.tblAttachments.Remove(tblattachments);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, tblattachments);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}