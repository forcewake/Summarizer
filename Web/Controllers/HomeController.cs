using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Models;

namespace Web.Controllers
{
    public class HomeController : Controller
    {

        private readonly ILanguageRepository languageRepository;


        // If you are using Dependency Injection, you can delete the following constructor
        public HomeController() : this(new LanguageRepository())
        {
        }

        public HomeController(ILanguageRepository languageRepository)
        {
			this.languageRepository = languageRepository;
        }

        public ActionResult Index()
        {
            ViewBag.PossibleLanguage = languageRepository.All;
            return View();
        }
    }
}
