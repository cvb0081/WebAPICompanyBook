using AspNetCoreRateLimit;
using Contracts;
using Entities.DataTransferObjects;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using NLog;
using Repository;
using Repository.DataShaping;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebAPIBook.ActionFilters;
using WebAPIBook.Extensions;
using WebAPIBook.Utility;

namespace WebAPIBook
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(),
                "/nlog.config"));
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureCors();
            services.ConfigureISSIntegration();
            services.ConfigureLoggerService();
            services.ConfigureSqlContext(Configuration);
            services.ConfigureRepositoryManager();
            services.AddAutoMapper(typeof(Startup));
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter= true;
            });
            services.AddScoped<ValidationFilterAttribute>();
            services.AddScoped<ValidateCompanyExistsAttribute>();
            services.AddScoped<ValidateEmployeeForCompanyExistsAttribute>();
            services.AddScoped<IDataShaper<EmployeeDto>,DataShaper<EmployeeDto>>();
            services.AddScoped<ValidateMediaTypeAttribute>();
            services.AddScoped<EmployeeLinks>();
            services.AddScoped<IAuthenticationManager, AuthenticationManager>();

            services.ConfigureVersioning();
            services.ConfigureResponseCaching();
            services.ConfigureHttpCacheHeaders();
            services.AddMemoryCache();

            services.AddAuthentication();
            services.ConfigureIdentity();
            services.ConfigureJWT(Configuration);

            services.ConfigureSwagger();

            services.ConfigureRateLimitingOptions();
            services.AddHttpContextAccessor();
            services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();

            services.AddControllers(config =>
            {
                config.RespectBrowserAcceptHeader = true;
                config.ReturnHttpNotAcceptable = true;
                config.CacheProfiles.Add("120SecondsDuration", new CacheProfile
                {
                    Duration = 120
                });
            }).AddNewtonsoftJson()
              .AddXmlDataContractSerializerFormatters()
              .AddCustomCSVFormatter();

            services.AddCustomMediaTypes();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
            ILoggerManager logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.ConfigureExceptionHandler(logger);

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseCors("CorsPolicy");

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.All
            });

            app.UseResponseCaching();

            app.UseHttpCacheHeaders();

            app.UseIpRateLimiting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseRouting();

            app.UseSwagger();
            app.UseSwaggerUI(s =>
            {
                s.SwaggerEndpoint("/swagger/v1/swagger.json", "Book API v1");
                s.SwaggerEndpoint("/swagger/v2/swagger.json", "Book API v2");
                
            });
        

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
