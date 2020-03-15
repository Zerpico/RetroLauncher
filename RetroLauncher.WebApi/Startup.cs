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
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RetroLauncher.DAL.Model;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.HttpOverrides;

namespace RetroLauncher.WebApi
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
            var constring = Configuration["ConnectionStrings:GamesLibraryConnection"];
            var opt = new DbContextOptionsBuilder<Model.DbLibraryGamesContext>().UseSqlServer(constring).Options;

            services.AddDbContext<Model.DbLibraryGamesContext>(option => option.UseSqlServer(constring));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // Set up custom content types - associating file extension to MIME type
            var provider = new FileExtensionContentTypeProvider();
            // Add new mappings
            provider.Mappings[".7z"] = "application/x-7z-compressed";

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\retrolauncher")),
                RequestPath = "/retrolauncher",
                ContentTypeProvider = provider
            });

           // app.UseHttpsRedirection();
            app.UseMvc();

        }


    }
}
