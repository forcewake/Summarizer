using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Web.Models;

namespace Web.Controllers
{
    public class SummarizerController : ApiController
    {

        	private readonly ILanguageRepository languageRepository;
		private readonly ITextRepository textRepository;

		// If you are using Dependency Injection, you can delete the following constructor
        public SummarizerController() : this(new LanguageRepository(), new TextRepository())
        {
        }

        public SummarizerController(ILanguageRepository languageRepository, ITextRepository textRepository)
        {
			this.languageRepository = languageRepository;
			this.textRepository = textRepository;
        }

        // GET api/Summarizer
        public IEnumerable<Text> GetTexts()
        {
            var texts = textRepository.AllIncluding(text => text.Language);
            return texts.AsEnumerable();
        }

        // GET api/Summarizer/5
        public Text GetText(int id)
        {
            Text text = textRepository.Find(id);
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

            try
            {
                textRepository.InsertOrUpdate(text);
                textRepository.Save();
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

            textRepository.InsertOrUpdate(text);
            textRepository.Save();

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, text);
            response.Headers.Location = new Uri(Url.Link("DefaultApi", new {id = text.TextId}));
            return response;
        }

        // DELETE api/Summarizer/5
        public HttpResponseMessage DeleteText(int id)
        {
            textRepository.Delete(id);
            textRepository.Save();

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                languageRepository.Dispose();
                textRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}