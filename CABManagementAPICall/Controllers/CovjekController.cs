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
    public class CovjekController : ApiController
    {
        private mirnesEntities1 db = new mirnesEntities1();

        // GET api/Covjek
        public IEnumerable<Covjek> GetCovjeks()
        {
            var covjek = db.Covjek.Include(c => c.Adresa);
            return covjek.AsEnumerable();
        }

        // GET api/Covjek/5
        public Covjek GetCovjek(int id)
        {
            Covjek covjek = db.Covjek.Find(id);
            if (covjek == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return covjek;
        }

        // PUT api/Covjek/5
        public HttpResponseMessage PutCovjek(int id, Covjek covjek)
        {
            if (ModelState.IsValid && id == covjek.id)
            {
                db.Entry(covjek).State = EntityState.Modified;

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

        // POST api/Covjek
        public HttpResponseMessage PostCovjek(Covjek covjek)
        {
            if (ModelState.IsValid)
            {
                db.Covjek.Add(covjek);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, covjek);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = covjek.id }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/Covjek/5
        public HttpResponseMessage DeleteCovjek(int id)
        {
            Covjek covjek = db.Covjek.Find(id);
            if (covjek == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.Covjek.Remove(covjek);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, covjek);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}