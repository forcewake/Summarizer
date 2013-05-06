using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class SummarizerController : ApiController
    {
        private readonly WebApplicationContext _db = new WebApplicationContext();

        // GET api/Summarizer
        public IEnumerable<Text> GetTexts()
        {
            var texts = _db.Texts.Include(t => t.Language);
            return texts.AsEnumerable();
        }

        // GET api/Summarizer/5
        public Text GetText(int id)
        {
            Text text = _db.Texts.Find(id);
            if (text == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return text;
        }

        // PUT api/Summarizer/5
        public HttpResponseMessage PutText(int id, Text text)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != text.TextId)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            _db.Entry(text).State = EntityState.Modified;

            try
            {
                _db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // POST api/Summarizer
        public HttpResponseMessage PostText(Text text)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
            _db.Texts.Add(text);
            _db.SaveChanges();

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, text);
            response.Headers.Location = new Uri(Url.Link("DefaultApi", new {id = text.TextId}));
            return response;
        }

        // DELETE api/Summarizer/5
        public HttpResponseMessage DeleteText(int id)
        {
            Text text = _db.Texts.Find(id);
            if (text == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            _db.Texts.Remove(text);

            try
            {
                _db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, text);
        }

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
            base.Dispose(disposing);
        }
    }
}