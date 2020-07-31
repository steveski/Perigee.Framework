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

namespace ExampleRestApi
{
    using Autofac;
    using Autofac.Core;
    using Autofac.Extensions.DependencyInjection;
    using Example.Domain.Customers.Commands;
    using Example.Domain.Customers.Views;
    using Microsoft.AspNetCore.Components.Authorization;
    using Perigee.Framework.Base.Transactions;
    using Perigee.Framework.Web.Extensions;
    using Perigee.Framework.Web.Middleware.Logging;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public ILifetimeScope AutofacContainer { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            AutofacContainer = app.ApplicationServices.GetAutofacRoot();

            
            // Add custom middleware
            // Wraps the return object with an ApiResponse Wrapper following RESTful Best Practices guide at http://www.restapitutorial.com/resources.html
            //app.UseApiResponseWrapperMiddleware();

            //app.UseMiddleware<LogRequestMiddleware>();
            //app.UseMiddleware<LogResponseMiddleware>();


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Example API");
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

            });

        }

        // Register Autofac dependencies here.
        // This is called by the AutofacServiceProviderFactory configured in Program.cs
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule<ExampleRestApiModule>();

        }

    }
}
