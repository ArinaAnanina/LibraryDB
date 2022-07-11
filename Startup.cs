using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.IO;
using LibraryDB.Services;
using NLog;
using NLog.Web;
using NLog.Extensions.Logging;

namespace LibraryDB
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(ApplicationContext.ConnectionString), ServiceLifetime.Singleton);
            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "LibraryDB", Version = "v1" });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            services.AddSingleton<AuthorService>();
            services.AddSingleton<BookService>();
            services.AddSingleton<Book_AuthorService>();
            services.AddSingleton<Book_PublicationTypeService>();
            services.AddSingleton<CategoryService>();
            services.AddSingleton<DepartmentService>();
            services.AddSingleton<EmployeeService>();
            services.AddSingleton<InstanceService>();
            services.AddSingleton<IssueService>();
            services.AddSingleton<PersonService>();
            services.AddSingleton<PostService>();
            services.AddSingleton<PublicationTypeService>();
            services.AddSingleton<PublishingHouseService>();
            services.AddSingleton<ReaderService>();
            services.AddSingleton<StatusTypeService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "LibraryDB v1"));
            }

            app.UseMiddleware<ErrorMiddleware>();
            app.UseMiddleware<LoggerMiddleware>();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
