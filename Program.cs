using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using LibraryDB.DB;
using NLog.Web;

namespace LibraryDB
{
    public class Program
    {
        public static void Main(string[] args)
        {
            NLogBuilder.ConfigureNLog("nlog.config");

            using (ApplicationContext db = new ApplicationContext())
            {
                //Person user1 = new Person { Name = "Tom", Surname = "Ders1", MiddleName = "wert", BirthDate = DateTime.Now, Domicile = "Russia", PhoneNumber = "89223368168" };
                //Person user2 = new Person { Name = "Tom", Surname = "Ders2", MiddleName = "wert", BirthDate = DateTime.Now, Domicile = "Russia", PhoneNumber = "89223368168" };
                //Person user3 = new Person { Name = "Tom", Surname = "Ders3", MiddleName = "wert", BirthDate = DateTime.Now, Domicile = "Russia", PhoneNumber = "89223368168" };
                //Person user4 = new Person { Name = "Tom", Surname = "Ders4", MiddleName = "wert", BirthDate = DateTime.Now, Domicile = "Russia", PhoneNumber = "89223368168" };

                //db.Person.Add(user1);
                //db.Person.Add(user2);
                //db.Person.Add(user3);
                //db.Person.Add(user4);
                //db.SaveChanges();
                //Console.WriteLine("Объекты успешно сохранены");

                //var users = db.Person.ToList();
                //Console.WriteLine("Список объектов:");
                //foreach (Person u in users)
                //{
                //    Console.WriteLine($"{u.Id}.{u.Name} - {u.Surname}");
                //}
            }
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(LogLevel.Trace);
                })
                .UseNLog();
    }
}
