using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.Comprehend;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Service.TextAnalysis.Configuration;
using Service.TextAnalysis.Contracts;
using Service.TextAnalysis.Security;
using Service.TextAnalysis.Services;
using Swashbuckle.AspNetCore.Swagger;

namespace Service.TextAnalysis
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            var authSettingsSection = Configuration.GetSection("Authentication");
            services.Configure<AuthenticationSettings>(authSettingsSection);

            var authSettings = authSettingsSection.Get<AuthenticationSettings>();
            var key = Encoding.ASCII.GetBytes(authSettings.Secret);
            services.AddAuthentication(_ =>
            {
                _.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                _.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(_ =>
            {
                _.RequireHttpsMetadata = false;
                _.SaveToken = true;
                _.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            services.AddSingleton<IConfiguration>(Configuration);

            // Create AWS comprehend client singleton.
            var awsOptions = Configuration.GetAWSOptions();
            var awsClient = awsOptions.CreateServiceClient<IAmazonComprehend>();
            services.AddSingleton<IAmazonComprehend>(awsClient);
            services.AddTransient<IAnalyzeSentimentService, AnalyzeSentimentService>();
            services.AddTransient<IUserService, UserService>();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Title = "Analyze Sentiment Service",
                    Version = "v1"
                });

                c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = "JWT Authorization",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });

                c.DocumentFilter<SecurityRequirementDocumentFilter>();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "version 1");
            });

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseCors(_ => _
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());

            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
