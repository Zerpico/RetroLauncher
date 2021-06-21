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
            services.AddControllers();

            #region Swagger
            services.AddSwaggerGen(c =>
            {
                //c.IncludeXmlComments(string.Format(@"{0}\RetroLauncherApi.xml", System.AppDomain.CurrentDomain.BaseDirectory));
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "RetroLauncher Api",
                });

                c.CustomSchemaIds(x => x.FullName);

                // This call remove version from parameter, without it we will have version as parameter 
                // for all endpoints in swagger UI
                //c.OperationFilter<RemoveVersionFromParameter>();

                // This make replacement of v{version:apiVersion} to real version of corresponding swagger doc.
                //c.DocumentFilter<ReplaceVersionWithExactValueInPath>();

            });
            #endregion
            
            #region Api Versioning
            // Add API Versioning to the Project
            services.AddApiVersioning(config =>
            {
                // Specify the default API Version as 1.0
                config.DefaultApiVersion = new ApiVersion(1, 0);
                // If the client hasn't specified the API version in the request, use the default API version number 
                config.AssumeDefaultVersionWhenUnspecified = true;
                // Advertise the API versions supported for the particular endpoint
                //config.ReportApiVersions = true;
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

            //app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "RetroLauncher Api");
            });

            // Set up custom content types - associating file extension to MIME type
            var provider = new FileExtensionContentTypeProvider();
            // Add new mappings
            provider.Mappings[".7z"] = "application/x-7z-compressed";

            string filesDirectory = Environment.GetEnvironmentVariable("ROMS_DIRECTORY");
            if (!Directory.Exists(Path.Combine(filesDirectory, "files")))
                Directory.CreateDirectory(Path.Combine(filesDirectory, "files"));

            app.UseDefaultFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(filesDirectory),
                ContentTypeProvider = provider
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
