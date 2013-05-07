using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace Web.Models
{ 
    public class TextRepository : ITextRepository
    {
        WebContext context = new WebContext();

        public IQueryable<Text> All
        {
            get { return context.Text; }
        }

        public IQueryable<Text> AllIncluding(params Expression<Func<Text, object>>[] includeProperties)
        {
            IQueryable<Text> query = context.Text;
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public Text Find(int id)
        {
            return context.Text.Find(id);
        }

        public void InsertOrUpdate(Text text)
        {
            if (text.TextId == default(int)) {
                // New entity
                context.Text.Add(text);
            } else {
                // Existing entity
                context.Entry(text).State = EntityState.Modified;
            }
        }

        public void Delete(int id)
        {
            var text = context.Text.Find(id);
            context.Text.Remove(text);
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

    public interface ITextRepository : IDisposable
    {
        IQueryable<Text> All { get; }
        IQueryable<Text> AllIncluding(params Expression<Func<Text, object>>[] includeProperties);
        Text Find(int id);
        void InsertOrUpdate(Text text);
        void Delete(int id);
        void Save();
    }
}