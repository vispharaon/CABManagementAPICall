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
    public class KorisniciController : ApiController
    {
        private mirnesEntities1 db = new mirnesEntities1();

        //// GET api/Korisnici
        //public IEnumerable<Korisnici> GetKorisnicis()
        //{
        //    return db.Korisnici.AsEnumerable();
        //}

        //// GET api/Korisnici/5
        //public Korisnici GetKorisnici(int id)
        //{
        //    Korisnici korisnici = db.Korisnici.Find(id);
        //    if (korisnici == null)
        //    {
        //        throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
        //    }

        //    return korisnici;
        //}

        // GET api/Korisnici/username/password
        public Korisnici GetKorisnici(string username, string password)
        {
            Korisnici korisnici = db.Korisnici.Where(x => x.username == username && x.password == password).FirstOrDefault();
            if (korisnici == null)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Nije pronađen korisnik sa unesenim podacima."));
            }

            return korisnici;
        }

        // PUT api/Korisnici/5
        public HttpResponseMessage PutKorisnici(int id, Korisnici korisnici)
        {
            if (ModelState.IsValid && id == korisnici.id)
            {
                db.Entry(korisnici).State = EntityState.Modified;

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

        // POST api/Korisnici
        public HttpResponseMessage PostKorisnici(Korisnici korisnici)
        {
            if (ModelState.IsValid)
            {
                db.Korisnici.Add(korisnici);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, korisnici);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = korisnici.id }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/Korisnici/5
        public HttpResponseMessage DeleteKorisnici(int id)
        {
            Korisnici korisnici = db.Korisnici.Find(id);
            if (korisnici == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.Korisnici.Remove(korisnici);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, korisnici);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}