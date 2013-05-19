using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TextSummarizationLibrary;
using TextSummarizationLibrary.Models;
using Web.Models;

namespace Web.Controllers
{
    public class SummarizerController : ApiController
    {
        private readonly ILanguageRepository _languageRepository;
        private readonly TextSummarizationLibrary.Controllers.SummarizerController _summarizerController;

        public SummarizerController()
            : this(new LanguageRepository(), new TextSummarizationLibrary.Controllers.SummarizerController())
        {
        }

        public SummarizerController(ILanguageRepository languageRepository, TextSummarizationLibrary.Controllers.SummarizerController summarizerController)
        {
            this._languageRepository = languageRepository;
            _summarizerController = summarizerController;
        }

        // POST api/Summarizer
        [HttpPost]
        public HttpResponseMessage Post(Text text)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            text.Language = _languageRepository.Find(text.LanguageId);

            object answer = null;
            if (text.Language != null)
            {
                var textModel = new TextModel
                    {
                        DisplayLines = text.DisplayLines,
                        Language = text.Language.ShortName,
                        Text = text.FullText
                    };
                
                    SummarizedDocument summarizedDocument = _summarizerController.Summarize(textModel);


                if (summarizedDocument != null)
                {
                    List<string> sentences = summarizedDocument.Sentences.Select(sentence => sentence.Trim()).ToList();
                    answer = new { keywords = summarizedDocument.Concepts, selectedSentences = sentences };
                }
            }

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, answer);
            return response;
        }
    }
}