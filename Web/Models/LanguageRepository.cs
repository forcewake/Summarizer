using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace Web.Models
{ 
    public class LanguageRepository : ILanguageRepository
    {
        WebContext context = new WebContext();

        public IQueryable<Language> All
        {
            get { return context.Language; }
        }

        public IQueryable<Language> AllIncluding(params Expression<Func<Language, object>>[] includeProperties)
        {
            IQueryable<Language> query = context.Language;
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public Language Find(int id)
        {
            return context.Language.Find(id);
        }

        public void InsertOrUpdate(Language language)
        {
            if (language.LanguageId == default(int)) {
                // New entity
                context.Language.Add(language);
            } else {
                // Existing entity
                context.Entry(language).State = EntityState.Modified;
            }
        }

        public void Delete(int id)
        {
            var language = context.Language.Find(id);
            context.Language.Remove(language);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Dispose() 
        {
            context.Dispose();
        }
    }

    public interface ILanguageRepository : IDisposable
    {
        IQueryable<Language> All { get; }
        IQueryable<Language> AllIncluding(params Expression<Func<Language, object>>[] includeProperties);
        Language Find(int id);
        void InsertOrUpdate(Language language);
        void Delete(int id);
        void Save();
    }
}