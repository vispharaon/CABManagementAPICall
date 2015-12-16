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
    public class CABCodeReviewController : ApiController
    {
        private cabmanagementEntities db = new cabmanagementEntities();

        // GET api/CABCodeReview
        public IEnumerable<tblCABCodeReview> GettblCABCodeReviews()
        {
            return db.tblCABCodeReview.AsEnumerable();
        }

        // GET api/CABCodeReview/5
        public tblCABCodeReview GettblCABCodeReview(int id)
        {
            tblCABCodeReview tblcabcodereview = db.tblCABCodeReview.Find(id);
            if (tblcabcodereview == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return tblcabcodereview;
        }

        // PUT api/CABCodeReview/5
        public HttpResponseMessage PuttblCABCodeReview(int id, tblCABCodeReview tblcabcodereview)
        {
            if (ModelState.IsValid && id == tblcabcodereview.ID)
            {
                db.Entry(tblcabcodereview).State = EntityState.Modified;

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

        // POST api/CABCodeReview
        public HttpResponseMessage PosttblCABCodeReview(tblCABCodeReview tblcabcodereview)
        {
            if (ModelState.IsValid)
            {
                db.tblCABCodeReview.Add(tblcabcodereview);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, tblcabcodereview);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = tblcabcodereview.ID }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/CABCodeReview/5
        public HttpResponseMessage DeletetblCABCodeReview(int id)
        {
            tblCABCodeReview tblcabcodereview = db.tblCABCodeReview.Find(id);
            if (tblcabcodereview == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.tblCABCodeReview.Remove(tblcabcodereview);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, tblcabcodereview);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}