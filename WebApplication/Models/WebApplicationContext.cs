﻿using System.Data.Entity;

namespace WebApplication.Models
{
    public class WebApplicationContext : DbContext
    {
        // В этот файл можно добавить пользовательский код. Изменения не будут перезаписаны.
        // 
        // Если требуется, чтобы платформа Entity Framework автоматически удаляла и формировала заново базу данных
        // при каждой смене схемы модели, добавьте следующий
        // код к методу Application_Start в файле Global.asax.
        // Примечание: в этом случае при каждой смене модели ваша база данных будет удалена и создана заново.
        // 
        // System.Data.Entity.Database.SetInitializer(new System.Data.Entity.DropCreateDatabaseIfModelChanges<WebApplication.Models.WebApplicationContext>());

        public WebApplicationContext() : base("name=WebApplicationContext")
        {
        }

        public DbSet<Text> Texts { get; set; }

        public DbSet<Language> Languages { get; set; }
    }
}
