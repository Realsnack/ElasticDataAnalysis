using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEnd.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace BackEnd
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _dateFormat = Configuration.GetValue<string>("DateTimeFormat");
        }

        public IConfiguration Configuration { get; }
        private static string _dateFormat;
        public static string LogTimeStamp() => DateTime.Now.ToString(_dateFormat) + ":\t";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Swagger implementation
            services.AddSwaggerGen(c => 
            {

                c.SwaggerDoc(Configuration.GetValue<string>("AppVersion"), new OpenApiInfo { 
                    Title = Configuration.GetValue<string>("AppTitle"), 
                    Version=Configuration.GetValue<string>("AppTitle")});
            });
            services.AddSingleton<IElastic, Elastic>();

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => 
            {
                c.SwaggerEndpoint($"/swagger/{Configuration.GetValue<string>("AppVersion")}/swagger.json", Configuration.GetValue<string>("AppTitle"));
            });

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
