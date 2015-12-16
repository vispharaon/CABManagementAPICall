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
    public class CABTaskaController : ApiController
    {
        private cabmanagementEntities db = new cabmanagementEntities();

        // GET api/CABTaska
        public IEnumerable<tblCABTaska> GettblCABTaskas()
        {
            var tblcabtaska = db.tblCABTaska.Include(t => t.tblDevelopers).Include(t => t.tblDevelopers1);
            return tblcabtaska.AsEnumerable();
        }

        // GET api/CABTaska/5
        public tblCABTaska GettblCABTaska(int id)
        {
            tblCABTaska tblcabtaska = db.tblCABTaska.Find(id);
            if (tblcabtaska == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return tblcabtaska;
        }

        // PUT api/CABTaska/5
        public HttpResponseMessage PuttblCABTaska(int id, tblCABTaska tblcabtaska)
        {
            if (ModelState.IsValid && id == tblcabtaska.CAB_Task_ID)
            {
                db.Entry(tblcabtaska).State = EntityState.Modified;

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

        // POST api/CABTaska
        public HttpResponseMessage PosttblCABTaska(tblCABTaska tblcabtaska)
        {
            if (ModelState.IsValid)
            {
                db.tblCABTaska.Add(tblcabtaska);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, tblcabtaska);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = tblcabtaska.CAB_Task_ID }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/CABTaska/5
        public HttpResponseMessage DeletetblCABTaska(int id)
        {
            tblCABTaska tblcabtaska = db.tblCABTaska.Find(id);
            if (tblcabtaska == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.tblCABTaska.Remove(tblcabtaska);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, tblcabtaska);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}