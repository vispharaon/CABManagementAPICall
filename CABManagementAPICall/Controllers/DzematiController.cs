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
    public class DzematiController : ApiController
    {
        private mirnesEntities1 db = new mirnesEntities1();

        // GET api/Dzemati
        public IEnumerable<Dzemati> GetDzematis()
        {
            var dzemati = db.Dzemati.Include(d => d.Adresa);
            return dzemati.AsEnumerable();
        }

        // GET api/Dzemati/5
        public Dzemati GetDzemati(int id)
        {
            Dzemati dzemati = db.Dzemati.Find(id);
            if (dzemati == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return dzemati;
        }

        // PUT api/Dzemati/5
        public HttpResponseMessage PutDzemati(int id, Dzemati dzemati)
        {
            if (ModelState.IsValid && id == dzemati.id)
            {
                db.Entry(dzemati).State = EntityState.Modified;

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

        // POST api/Dzemati
        public HttpResponseMessage PostDzemati(Dzemati dzemati)
        {
            if (ModelState.IsValid)
            {
                db.Dzemati.Add(dzemati);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, dzemati);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = dzemati.id }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/Dzemati/5
        public HttpResponseMessage DeleteDzemati(int id)
        {
            Dzemati dzemati = db.Dzemati.Find(id);
            if (dzemati == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.Dzemati.Remove(dzemati);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, dzemati);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}