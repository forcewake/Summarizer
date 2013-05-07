using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Models;

namespace Web.Controllers
{   
    public class LanguageController : Controller
    {
		private readonly ILanguageRepository languageRepository;

		// If you are using Dependency Injection, you can delete the following constructor
        public LanguageController() : this(new LanguageRepository())
        {
        }

        public LanguageController(ILanguageRepository languageRepository)
        {
			this.languageRepository = languageRepository;
        }

        //
        // GET: /Language/

        public ViewResult Index()
        {
            return View(languageRepository.All);
        }

        //
        // GET: /Language/Details/5

        public ViewResult Details(int id)
        {
            return View(languageRepository.Find(id));
        }

        //
        // GET: /Language/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Language/Create

        [HttpPost]
        public ActionResult Create(Language language)
        {
            if (ModelState.IsValid) {
                languageRepository.InsertOrUpdate(language);
                languageRepository.Save();
                return RedirectToAction("Index");
            } else {
				return View();
			}
        }
        
        //
        // GET: /Language/Edit/5
 
        public ActionResult Edit(int id)
        {
             return View(languageRepository.Find(id));
        }

        //
        // POST: /Language/Edit/5

        [HttpPost]
        public ActionResult Edit(Language language)
        {
            if (ModelState.IsValid) {
                languageRepository.InsertOrUpdate(language);
                languageRepository.Save();
                return RedirectToAction("Index");
            } else {
				return View();
			}
        }

        //
        // GET: /Language/Delete/5
 
        public ActionResult Delete(int id)
        {
            return View(languageRepository.Find(id));
        }

        //
        // POST: /Language/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            languageRepository.Delete(id);
            languageRepository.Save();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                languageRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

