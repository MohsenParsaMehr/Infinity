using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using WhiteDream.Models;

namespace WhiteDream.Controllers
{
    public class PersonsController : ApiController
    {
        private PersonsContext db = new PersonsContext();
        private YJEntities entities = new YJEntities();

        // GET api/Persons
        public IEnumerable<Persons> GetPersons()
        {
            return db.Persons.AsEnumerable();
        }

        // GET api/Persons/5
        public Persons GetPersons(int id)
        {
            Persons persons = db.Persons.Find(id);
            if (persons == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return persons;
        }

        // PUT api/Persons/5
        public HttpResponseMessage PutPersons(int id, Persons persons)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != persons.Id)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            db.Entry(persons).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // POST api/Persons
        public HttpResponseMessage PostPersons(Persons person)
        {
            if (ModelState.IsValid)
            {
                HttpResponseMessage response;
                try
                {
                    if (person.CityRef == 0 || person.ProvinceRef == 0)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "محل مشخص نشده است");
                    }
                    person.Status = Constants.RecordStatus.ACTIVE;
                    person.InsertDate =  DateTime.Now;
                    person.UpdateDate = DateTime.Now;

                    entities.Persons.Add(person);
                    entities.SaveChanges();

                    return new HttpResponseMessage()
                    {
                        Content = new StringContent(person.Id.ToString(), Encoding.UTF8, "text/plain")
                    };
                }
                catch (EntityException ee)
                {
                    response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ee);
                    return response;
                }
                catch (Exception e)
                {
                    response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
                    return response;
                    //throw e;
                }
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        // DELETE api/Persons/5
        public HttpResponseMessage DeletePersons(int id)
        {
            Persons persons = db.Persons.Find(id);
            if (persons == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.Persons.Remove(persons);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, persons);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}