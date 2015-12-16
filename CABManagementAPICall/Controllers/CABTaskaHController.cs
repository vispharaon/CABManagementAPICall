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
    public class CABTaskaHController : ApiController
    {
        private cabmanagementEntities db = new cabmanagementEntities();

        // GET api/CABTaskaH
        public IEnumerable<tblCABTaskaH> GettblCABTaskaHs()
        {
            return db.tblCABTaskaH.AsEnumerable();
        }

        // GET api/CABTaskaH/5
        public tblCABTaskaH GettblCABTaskaH(int id)
        {
            tblCABTaskaH tblcabtaskah = db.tblCABTaskaH.Find(id);
            if (tblcabtaskah == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return tblcabtaskah;
        }

        // PUT api/CABTaskaH/5
        public HttpResponseMessage PuttblCABTaskaH(int id, tblCABTaskaH tblcabtaskah)
        {
            if (ModelState.IsValid && id == tblcabtaskah.CAB_Task_ID)
            {
                db.Entry(tblcabtaskah).State = EntityState.Modified;

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

        // POST api/CABTaskaH
        public HttpResponseMessage PosttblCABTaskaH(tblCABTaskaH tblcabtaskah)
        {
            if (ModelState.IsValid)
            {
                db.tblCABTaskaH.Add(tblcabtaskah);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, tblcabtaskah);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = tblcabtaskah.CAB_Task_ID }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/CABTaskaH/5
        public HttpResponseMessage DeletetblCABTaskaH(int id)
        {
            tblCABTaskaH tblcabtaskah = db.tblCABTaskaH.Find(id);
            if (tblcabtaskah == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.tblCABTaskaH.Remove(tblcabtaskah);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, tblcabtaskah);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}