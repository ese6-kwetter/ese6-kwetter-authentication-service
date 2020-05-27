using System;
using System.Text;
using MessageBroker;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RabbitMQ.Client;
using Sentry.Extensibility;
using UserMicroservice.Helpers;
using UserMicroservice.Repositories;
using UserMicroservice.Services;
using UserMicroservice.Settings;

namespace UserMicroservice
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        private bool _running = true;
        
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region Settings

            // Configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection(nameof(AppSettings));
            services.Configure<AppSettings>(appSettingsSection);

            var databaseSettingsSection = Configuration.GetSection(nameof(DatabaseSettings));
            services.Configure<DatabaseSettings>(databaseSettingsSection);

            var messageQueueSection = Configuration.GetSection(nameof(MessageQueueSettings));
            services.Configure<MessageQueueSettings>(Configuration.GetSection(nameof(MessageQueueSettings)));

            #endregion

            #region Swagger.io

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Values Api"
                });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });
            });

            #endregion

            // Configure DI for database settings
            services.AddSingleton<IDatabaseSettings>(serviceProvider =>
                serviceProvider.GetRequiredService<IOptions<DatabaseSettings>>().Value);

            // Configure RabbitMQ
            services.AddMessagePublisher(messageQueueSection.Get<MessageQueueSettings>().Uri);

            // Configure DI for application services
            services.AddTransient<ILoginService, LoginService>();
            services.AddTransient<IRegisterService, RegisterService>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IHashGenerator, HashGenerator>();
            services.AddTransient<ITokenGenerator, TokenGenerator>();
            services.AddTransient<IRegexValidator, RegexValidator>();

            // Configure JWT authentication
            var appSettings = appSettingsSection.Get<AppSettings>();
            var signingKey = Encoding.ASCII.GetBytes(appSettings.JwtSecret);

            #region Authentication

            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                }
            ).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(signingKey),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
            // .AddGoogle(options =>
            // {
            //     var googleAuthNSection = Configuration.GetSection("Authentication:Google");
            //
            //     options.ClientId = googleAuthNSection["ClientId"];
            //     options.ClientSecret = googleAuthNSection["ClientSecret"];
            // });

            #endregion

            services.AddCors();
            services.AddControllers();
            services.AddRouting(options => options.LowercaseUrls = true);

            #region Health Checks with dependencies

            services.AddHealthChecks()
                .AddMongoDb(
                    databaseSettingsSection.Get<DatabaseSettings>().ConnectionString,
                    tags: new[] {"services"}
                )
                .AddRabbitMQ(
                    new Uri(messageQueueSection.Get<MessageQueueSettings>().Uri),
                    tags: new[] {"services"}
            );

            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                
                // Swagger.io
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "User Microservice");
                });
            }

            // Global CORS policy
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
            
            // Routing with authentication and authorization
            // The call to UseAuthorization should appear between app.UseRouting()
            // and app.UseEndpoints(..) for authorization to be correctly evaluated.
            app.UseRouting();
            
            app.UseAuthentication();
            app.UseAuthorization();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            #region Health Checks
            
            app.UseHealthChecks("/healthy", new HealthCheckOptions
            {
                Predicate = r => r.Name.Contains("healthy")
            });
            app.UseHealthChecks("/ready", new HealthCheckOptions
            {
                Predicate = r => r.Tags.Contains("services")
            });
            
            app.Map("/switch", appBuilder =>
            {
                appBuilder.Run(async context =>
                {
                    _running = !_running;
                    await context.Response.WriteAsync($"{Environment.MachineName} running {_running}");
                });
            });
            
            #endregion
        }
    }
}
