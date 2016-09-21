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
    public class AdresaController : ApiController
    {
        private mirnesEntities1 db = new mirnesEntities1();

        // GET api/Adresa
        public IEnumerable<Adresa> GetAdresas()
        {
            return db.Adresa.AsEnumerable();
        }

        // GET api/Adresa/5
        public Adresa GetAdresa(int id)
        {
            Adresa adresa = db.Adresa.Find(id);
            if (adresa == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return adresa;
        }

        // PUT api/Adresa/5
        public HttpResponseMessage PutAdresa(int id, Adresa adresa)
        {
            if (ModelState.IsValid && id == adresa.id)
            {
                db.Entry(adresa).State = EntityState.Modified;

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

        // POST api/Adresa
        public HttpResponseMessage PostAdresa(Adresa adresa)
        {
            if (ModelState.IsValid)
            {
                db.Adresa.Add(adresa);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, adresa);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = adresa.id }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/Adresa/5
        public HttpResponseMessage DeleteAdresa(int id)
        {
            Adresa adresa = db.Adresa.Find(id);
            if (adresa == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.Adresa.Remove(adresa);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, adresa);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}