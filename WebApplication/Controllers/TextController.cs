using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TextSummarizationLibrary;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class TextController : Controller
    {
        private WebApplicationContext db = new WebApplicationContext();

        //
        // GET: /Text/

        public ActionResult Index()
        {
            var texts = db.Texts.Include(t => t.Language);
            return View(texts.ToList());
        }

        //
        // GET: /Text/Details/5

        public ActionResult Details(int id = 0)
        {
            Text text = db.Texts.Find(id);
            if (text == null)
            {
                return HttpNotFound();
            }
            return View(text);
        }

        //
        // GET: /Text/Create

        public ActionResult Create()
        {
            ViewBag.LanguageId = new SelectList(db.Languages, "LanguageId", "FullName");
            return View();
        }

        //
        // POST: /Text/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Text text)
        {


            if (ModelState.IsValid)
            {
                text.Language = db.Languages.Find(text.LanguageId);
                SummarizedDocument doc = new Summarizer().Summarize(
                    new TextModel
                    {
                        DisplayLines = text.DisplayLines,
                        Language = text.Language.ShortName,
                        Text = text.FullText
                    });

                text.ShortText = string.Join(Environment.NewLine, doc.Sentences);
                db.Texts.Add(text);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.LanguageId = new SelectList(db.Languages, "LanguageId", "FullName", text.LanguageId);
            return View(text);
        }

        //
        // GET: /Text/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Text text = db.Texts.Find(id);
            if (text == null)
            {
                return HttpNotFound();
            }
            ViewBag.LanguageId = new SelectList(db.Languages, "LanguageId", "FullName", text.LanguageId);
            return View(text);
        }

        //
        // POST: /Text/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Text text)
        {
            if (ModelState.IsValid)
            {
                db.Entry(text).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LanguageId = new SelectList(db.Languages, "LanguageId", "FullName", text.LanguageId);
            return View(text);
        }

        //
        // GET: /Text/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Text text = db.Texts.Find(id);
            if (text == null)
            {
                return HttpNotFound();
            }
            return View(text);
        }

        //
        // POST: /Text/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Text text = db.Texts.Find(id);
            db.Texts.Remove(text);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}