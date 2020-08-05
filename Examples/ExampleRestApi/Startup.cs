using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ExampleRestApi
{
    using Autofac;
    using Autofac.Extensions.DependencyInjection;
    using AutoMapper;
    using Example.Domain.Addresses.Commands;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Diagnostics.HealthChecks;
    using Perigee.Framework.EntityFramework;
    using System.Reflection;

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
            services.AddAutoMapper(typeof(HandleCreateAddressCommand).GetTypeInfo().Assembly);
            services.AddControllers();
            services.AddSwaggerGen();

            services.AddTransient<IDatabaseConfig, DatabaseConfig>();

            services.AddHealthChecks()
                .AddCheck("HealthCheck", () => HealthCheckResult.Healthy(DateTime.Now.ToLongTimeString()));

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
                endpoints.MapHealthChecks("/health");

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
