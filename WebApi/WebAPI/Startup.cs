using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application;
using Persistence;
using RetroLauncher.WebAPI.Filters;
using Microsoft.AspNetCore.StaticFiles;
using System.IO;
using Microsoft.Extensions.FileProviders;
using System.ComponentModel;
using System.Reflection;
using VueCliMiddleware;

namespace RetroLauncher.WebAPI
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
            services.AddControllersWithViews();
            services.AddSpaStaticFiles(configuration: options => { options.RootPath = "wwwroot"; });

            #region Swagger
            services.AddSwaggerGen(c =>
            {
                //c.IncludeXmlComments(string.Format(@"{0}\RetroLauncherApi.xml", System.AppDomain.CurrentDomain.BaseDirectory));
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "RetroLauncher Api",
                    Contact = new OpenApiContact()
                    {
                        Email = "zerpico@yandex.ru",
                        Url = new Uri("mailto:zerpico@yandex.ru")
                    }
                });

                c.CustomSchemaIds(x => x.Name);

                var fileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var filePath = Path.Combine(AppContext.BaseDirectory, fileName);
                c.IncludeXmlComments(filePath);   
            });
            #endregion
            
            services.AddApplication();
            services.AddPersistence(Configuration);            
            services.AddControllersWithViews()
                .AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseHsts();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Api RetroLauncher");                
            });

            #region Static File Options
            // Set up custom content types - associating file extension to MIME type
            var provider = new FileExtensionContentTypeProvider();
            provider.Mappings[".7z"] = "application/x-7z-compressed";
            // Init FileServerOptions
            var FilesPath = Configuration["FilesPath"];
            var fileServerOptions = new FileServerOptions
            {
                FileProvider = new PhysicalFileProvider(FilesPath)
            };
            fileServerOptions.StaticFileOptions.ContentTypeProvider = provider;
            #endregion

            //Enable CORS
            app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseStaticFiles(new StaticFileOptions
            {
                ContentTypeProvider = provider
            });
            app.UseFileServer(fileServerOptions);
            app.UseSpaStaticFiles();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                //endpoints.MapFallbackToController("Index", "Home");
            });

            app.UseSpa(spa =>
            {
                if (env.IsDevelopment())
                    spa.Options.SourcePath = "wwwroot/";
                else
                    spa.Options.SourcePath = "dist";

                if (env.IsDevelopment())
                {
                    spa.UseVueCli();
                }

                /*if (env.IsDevelopment())
                {

                    // run npm process with client app
                    if (mode == "start")
                    {
                        spa.UseVueCli(npmScript: "serve", port: port, forceKill: true, https: https);
                    }

                    // if you just prefer to proxy requests from client app, use proxy to SPA dev server instead,
                    // app should be already running before starting a .NET client:
                    // run npm process with client app
                    if (mode == "attach")
                    {
                        spa.UseProxyToSpaDevelopmentServer($"{(https ? "https" : "http")}://localhost:{port}"); // your Vue app port
                    }
                }*/
            });
        }
    }
}
