using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TextSummarizationLibrary;
using Web.Models;

namespace Web.Controllers
{   
    public class TextController : Controller
    {
		private readonly ILanguageRepository languageRepository;
		private readonly ITextRepository textRepository;

		// If you are using Dependency Injection, you can delete the following constructor
        public TextController() : this(new LanguageRepository(), new TextRepository())
        {
        }

        public TextController(ILanguageRepository languageRepository, ITextRepository textRepository)
        {
			this.languageRepository = languageRepository;
			this.textRepository = textRepository;
        }

        //
        // GET: /Text/

        public ViewResult Index()
        {
            return View(textRepository.AllIncluding(text => text.Language));
        }

        //
        // GET: /Text/Details/5

        public ViewResult Details(int id)
        {
            return View(textRepository.Find(id));
        }

        //
        // GET: /Text/Create

        public ActionResult Create()
        {
			ViewBag.PossibleLanguage = languageRepository.All;
            return View();
        } 

        //
        // POST: /Text/Create

        [HttpPost]
        public ActionResult Create(Text text)
        {
            if (ModelState.IsValid) {
                textRepository.InsertOrUpdate(text);
                textRepository.Save();
                text.Language = languageRepository.Find(text.LanguageId);
                text.ShortText = string.Join("\n", new Summarizer()
                                                       .Summarize(new TextModel
                                                       {
                                                           DisplayLines = text.DisplayLines,
                                                           Language = text.Language.ShortName,
                                                           Text = text.FullText
                                                       }).Sentences);
                textRepository.Save();
                return RedirectToAction("Index");
            } else {
				ViewBag.PossibleLanguage = languageRepository.All;
				return View();
			}
        }
        
        //
        // GET: /Text/Edit/5
 
        public ActionResult Edit(int id)
        {
			ViewBag.PossibleLanguage = languageRepository.All;
             return View(textRepository.Find(id));
        }

        //
        // POST: /Text/Edit/5

        [HttpPost]
        public ActionResult Edit(Text text)
        {
            if (ModelState.IsValid) {
                textRepository.InsertOrUpdate(text);
                textRepository.Save();
                return RedirectToAction("Index");
            } else {
				ViewBag.PossibleLanguage = languageRepository.All;
				return View();
			}
        }

        //
        // GET: /Text/Delete/5
 
        public ActionResult Delete(int id)
        {
            return View(textRepository.Find(id));
        }

        //
        // POST: /Text/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            textRepository.Delete(id);
            textRepository.Save();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                languageRepository.Dispose();
                textRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

