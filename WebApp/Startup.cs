using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Serilog;
using WebApp.Hubs;
using WebApp.Services;
using WebApp.Services.Interfaces;

namespace WebApp
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
            services.AddDbContext<ApplicationDbContext>(options =>
                // options.UseSqlite(
                // Configuration.GetConnectionString("SQLite")));
                options.UseMySql(Configuration.GetConnectionString("MySQL"), ServerVersion.AutoDetect(Configuration.GetConnectionString("MySQL"))));
                
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddDefaultIdentity<ApplicationUser>(options =>
                {
                    options.SignIn.RequireConfirmedAccount = false;
                    options.SignIn.RequireConfirmedEmail = false;
                    options.SignIn.RequireConfirmedPhoneNumber = false;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddIdentityServer(Configuration.GetSection("IdentityServer"))
                .AddApiAuthorization<ApplicationUser, ApplicationDbContext>();

            services.AddAuthentication()
                .AddIdentityServerJwt();
            
            services.TryAddEnumerable(
                ServiceDescriptor.Singleton<IPostConfigureOptions<JwtBearerOptions>, ConfigureJwtBearerOptions>());

            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddSignalR();

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });

            // https://www.rightpoint.com/rplabs/2019/07/swagger/
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "30secs api",
                    Version = "v1",
                });
                
                var securityScheme = new OpenApiSecurityScheme
                {
                    Name = "JWT Authentication",
                    Description = "Enter JWT Bearer token **_only_**",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer", // must be lower case
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme,
                    },
                };
                
                options.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {securityScheme, Array.Empty<string>()},
                });
                
                var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                var commentsFileName = Assembly.GetEntryAssembly()?.GetName().Name + ".xml";
                var commentsFile = Path.Combine(baseDirectory, commentsFileName);
                options.IncludeXmlComments(commentsFile);
            });

            services
                .AddSingleton<ISymbolInMemoryStorageService, SymbolInMemoryStorageService>()
                .AddTransient<IContactLogicService, ContactLogicService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();
            app.UseSerilogRequestLogging();
            app.UseRouting();

            app.UseAuthentication();
            app.UseIdentityServer();
            app.UseAuthorization();
            
            // https://www.rightpoint.com/rplabs/2019/07/swagger/
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("v1/swagger.json", "30secs api v1");
            });
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<ChatHub>("chatHub");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
        }
    }
}
